// Login ajax

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