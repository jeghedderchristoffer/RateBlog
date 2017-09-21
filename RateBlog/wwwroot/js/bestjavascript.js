// Update navbar to mobile version 

$(document).ready(function () {
    if (window.innerWidth <= 767) {
        $("#dropdownPc").hide();
        $("#dropdownMobile").show();
    }
    else {
        $("#dropdownPc").show();
        $("#dropdownMobile").hide();
    }

    $(window).resize(function () {
        if (window.innerWidth <= 767) {
            $("#dropdownPc").hide();
            $("#dropdownMobile").show();
        }
        else {
            $("#dropdownPc").show();
            $("#dropdownMobile").hide();
        }
    });
});





// Rating stars

$(document).ready(function () {
    $(".rating label").click(function () {
        $(this).parent().find("label").css({ "color": "#FD4" });
        $(this).css({ "color": "#FD4" });
        $(this).nextAll().css({ "color": "#DCDCDC" });

        if ($(this).parent().attr("id") === "opførselCheckBox") {
            $("#opførselCheck").fadeIn(400);
        }
        if ($(this).parent().attr("id") === "kvalitetCheckBox") {
            $("#kvalitetCheck").fadeIn(400);
        }
        if ($(this).parent().attr("id") === "troværdighedCheckBox") {
            $("#troværdighedCheck").fadeIn(400);
        }
        if ($(this).parent().attr("id") === "interaktionCheckBox") {
            $("#interaktionCheck").fadeIn(400);
        }
    });
});


// DatePicker

$(document).ready(function () {
    $("input[name=Birthday]").datepicker({
        dateFormat: "dd-mm-yy",
        minDate: new Date(1910, 0, 1),
        maxDate: new Date(2017, 0, 1),
        yearRange: '1910:2017',
        changeYear: true,
        changeMonth: true,
        defaultDate: new Date("March 21, 2000")

    });
});


// Log out

$(document).ready(function () {
    $("#logOut").click(function () {
        $("#logoutForm").submit();
    });
    $("#logOutMobile").click(function () {
        $("#logoutForm").submit();
    });
});

// Login

$(document).ready(function () {

    $("#email").keydown(function (e) {
        if (e.keyCode === 13) {
            loginFunction();
        }
    });

    $("#password").keydown(function (e) {
        if (e.keyCode === 13) {
            loginFunction();
        }
    });

    $("#loginBtn").click(function () {
        loginFunction();
    });
});

function loginFunction() {
    var errorMsgTop = $("#errorMsgTop");
    var errorMsgBot = $("#errorMsgBot");
    var returnNothing = false;

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
        url: "/Account/LoginAjax",
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
}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

// Login slider

$(document).ready(function () {

    var isOpenLogin = false;

    $("#emailLoginButton").click(function () {
        if (isOpenLogin) {
            $("#emailLogin").slideUp();
            isOpenLogin ^= true;
        }
        else {
            $("#emailLogin").slideDown();
            isOpenLogin ^= true;
        }
    });

});


function updateNotification(count) {
    var feedbackNumber = count;
    var bagdes = document.getElementsByClassName("navbarBagde");

    if (feedbackNumber === 0) {
        for (var i = 0; i < bagdes.length; i++) {
            bagdes[i].innerHTML = '';
            bagdes[i].style.opacity = "0";
        }
    } else {
        for (var i = 0; i < bagdes.length; i++) {
            bagdes[i].innerHTML = feedbackNumber;
        }
    }
}

$(document).ready(function () {
    var id = $("#applicationUserId").val();

    if (id != null) {
        $.ajax({
            cache: false,
            url: "/Manage/GetUnreadFeedback",
            method: "GET",
            dataType: 'json',
            data: {
                id: id
            },
            success: function (result) {
                updateNotification(result);
            },
            error: function () {
                console.log("fail");
            }
        });
    }
});