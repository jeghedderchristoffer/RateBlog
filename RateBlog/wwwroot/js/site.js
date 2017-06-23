﻿// Login ajax

$(document).ready(function () {
    var errorMsgTop = $("#errorMsgTop");
    var errorMsgBot = $("#errorMsgBot");
    var returnNothing = false;

    $("#loginBtn").click(function () {
        //errorMsgTop.text("");
        //errorMsgBot.text(""); 

        //collect the user data
        var data = {};
        data.Email = $("#email").val();
        data.Password = $("#password").val();
        var token = $('input[name="__RequestVerificationToken"]').val();

        if (!isEmail(data.Email)) {
            $("#email").focus();
            errorMsgTop.text("Dette er ikke en gyldig email.");
            if (data.Password === '') {
                errorMsgBot.text("Du mangler kodeord.");
            }
            else {
                errorMsgBot.text("");
            }
            return;
        }

        $.ajax({
            url: "/Account/Login",
            method: "POST",
            data: {
                model: data,
                __RequestVerificationToken: token,
                returnUrl: "Home/Index"   // you can modify the returnUrl value here
            },
            success: function (result) {
                var JSONString = JSON.stringify(result);
                var resultObj = JSON.parse(JSONString);

                console.log(data.Password);

                if (resultObj.result === "Missing") {
                    if (data.Password === '') {
                        $("#password").focus();
                        errorMsgBot.text("Du mangler kodeord.");
                    }
                    if (isEmail(data.Email)) {
                        errorMsgTop.text("");
                    }
                }
                else {
                    errorMsgBot.text("");
                }

                if (resultObj.result === "Wrong") {
                    $("#email").focus();
                    errorMsgTop.text("Brugernavn eller kodeordet er forkert.");
                }

                if (resultObj.result === "Accepted") {
                    location.reload();
                }

            },
            error: function () {
                console.log("fail");
            }
        });
    });
});

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

$(document).ready(function () {
    $("#forgotPasswordBtn").click(function () {
        $("#forgotPassword").css("display", "block");
    });

    $('#loginModal').on('hidden.bs.modal', function () {
        $("#forgotPassword").css("display", "none");
    });
});

// Opret ajax

$(document).ready(function () {
    var opretErrorMsg = $("#opretErrorMsgTop"); 

    $("#opretBtn").click(function () {

        //collect the user data
        var data = {};
        data.FirstName = $("#opretFirstName").val();
        data.LastName = $("#opretLastName").val();
        data.Email = $("#opretEmail").val();
        data.Password = $("#opretPassword").val();
        data.ConfirmPassword = $("#opretPassword2").val();
        var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: "/Account/Register",
            method: "POST",
            data: {
                model: data,
                __RequestVerificationToken: token,
                returnUrl: "Home/Index"   // you can modify the returnUrl value here
            },
            success: function (result) {
                console.log(result);

                if (result.sucess === true) {
                    location.reload();
                    return;
                }

                for (var i = 0; i < result.error.length; i++) {
                    console.log(result.error[i].errors[0].errorMessage);
                    var error = result.error[i].errors[0].errorMessage; 
                    if (error === 'Missing') {
                        opretErrorMsg.text("Du skal udfylde alle felterne.");
                        return;
                    } 
                    else if (error === 'Email') {
                        opretErrorMsg.text("Dette er ikke en gyldig email.");
                        $("#opretEmail").focus();
                        return;
                    }
                    else if (error === 'Password') {
                        opretErrorMsg.text("Kodeordet skal være minimum 6 karaktere langt.");
                        return;
                    }
                    else if (error === 'NoMatch') {
                        opretErrorMsg.text("De 2 kodeord er forskellige.");
                        return;
                    }
                    else if (error.indexOf("User name") !== -1) {
                        opretErrorMsg.text("Der findes allerede en bruger med denne email.");
                        return;
                    }

                }

            },
            error: function () {
                console.log("fail");
            }
        });
    });
});