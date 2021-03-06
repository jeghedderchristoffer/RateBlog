﻿// Login ajax

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
}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}



// Log af

$(document).ready(function () {
    $("#logOut").click(function () {
        $("#logoutForm").submit();
    });
    $("#logOutMobile").click(function () {
        $("#logoutForm").submit();
    });
});

// Update navbar to mobile version 

$(document).ready(function () {
    if ($(window).width() <= 767) {
        $("#dropdownPc").hide();
        $("#dropdownMobile").show();
    }
    else {
        $("#dropdownPc").show();
        $("#dropdownMobile").hide();
    }

    $(window).resize(function () {
        if ($(window).width() <= 767) {
            $("#dropdownPc").hide();
            $("#dropdownMobile").show();
        }
        else {
            $("#dropdownPc").show();
            $("#dropdownMobile").hide();
        }
    });
});

// Rediger profil

$(document).ready(function () {
    var influenterBtn = $("#redigerInfluenterBtn"),
        fanBtn = $("#redigerFanBtn"),
        influenterContainer = $("#redigerInfluenterContainer"),
        fanContainer = $("#redigerFanContainer");

    if ($("#redigerInfluenterBtn").hasClass("active")) {
        fanContainer.hide();
    }
    else {
        influenterContainer.hide();
    }

    influenterBtn.click(function () {
        influenterBtn.addClass("active");
        fanBtn.removeClass("active");
        influenterContainer.show();
        fanContainer.hide();
    });

    fanBtn.click(function () {
        influenterBtn.removeClass("active");
        fanBtn.addClass("active");
        fanContainer.show();
        influenterContainer.hide();
    });

    // Text efter inputs social media

    var FacebookInput = $("#editFacebook"),
        FacebookText = $("#editFacebookText"),
        InstagramInput = $("#editInstagram"),
        InstagramText = $("#editInstagramText"),
        YoutubeInput = $("#editYoutube"),
        YoutubeText = $("#editYoutubeText"),
        SnapchatInput = $("#editSnapchat"),
        SnapchatText = $("#editSnapchatText"),
        TwitterInput = $("#editTwitter"),
        TwitterText = $("#editTwitterText"),
        TwitchInput = $("#editTwitch"),
        TwitchText = $("#editTwitchText"),
        WebsiteInput = $("#editWebsite"),
        WebsiteText = $("#editWebsiteText"),
        SecondYoutubeInput = $("#editSecondYoutube"),
        SecondYoutubeText = $("#editSecondYoutubeText");

    if (FacebookInput.val() !== "") {
        FacebookText.text($("#editFacebook").val());
    }

    if (InstagramInput.val() !== "") {
        InstagramText.text("www.instagram.com/" + InstagramInput.val());
    }

    if (YoutubeInput.val() !== "") {
        YoutubeText.text(YoutubeInput.val());
    }

    if (SecondYoutubeInput.val() !== "") {
        SecondYoutubeText.text(SecondYoutubeInput.val());
    }

    if (SnapchatInput.val() !== "") {
        SnapchatText.text("www.snapchat.com/add/" + SnapchatInput.val());
    }

    if (TwitterInput.val() !== "") {
        TwitterText.text("www.twitter.com/" + TwitterInput.val());
    }

    if (TwitchInput.val() !== "") {
        TwitchText.text(TwitchInput.val());
    }

    if (WebsiteInput.val() !== "") {
        WebsiteText.text("http://" + WebsiteInput.val());
    }

    WebsiteInput.keyup(function () {
        WebsiteText.text("http://" + WebsiteInput.val());
    });

    TwitchInput.keyup(function () {
        TwitchText.text(TwitchInput.val());
    });

    TwitterInput.keyup(function () {
        TwitterText.text("www.twitter.com/" + TwitterInput.val());
    });

    SnapchatInput.keyup(function () {
        SnapchatText.text("www.snapchat.com/add/" + SnapchatInput.val());
    });

    FacebookInput.keyup(function () {
        FacebookText.text(FacebookInput.val());
    });

    InstagramInput.keyup(function () {
        InstagramText.text("www.instagram.com/" + InstagramInput.val());
    });

    YoutubeInput.keyup(function () {
        YoutubeText.text(YoutubeInput.val());
    });

    SecondYoutubeInput.keyup(function () {
        SecondYoutubeText.text(SecondYoutubeInput.val());
    });

});

//Rating Stars

$(document).ready(function () {
    $('[data-toggle="popover"]').popover({
        container: "body"
    });
});

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


// Ajax til search forslag

$(document).ready(function () {

    //setup before functions
    var typingTimer;                //timer identifier
    var doneTypingInterval = 100;  //time in ms, 5 second for example
    var $input = $('#search');

    //on keyup, start the countdown
    $input.on('keyup', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(doneTyping, doneTypingInterval);
    });

    //on keydown, clear the countdown 
    $input.on('keydown', function () {
        clearTimeout(typingTimer);
    });

    //user is "finished typing," do something
    function doneTyping() {

        var searchResult = $("#search").val();

        $.ajax({
            url: "/Home/SearchHelp",
            method: "GET",
            data: {
                search: searchResult
            },
            success: function (result) {
                if (result.trim()) {
                    $("#searchSug").html(result);
                }
                else {
                    $("#searchSug").html("");
                }
            },
            error: function () {
                console.log("fail");
            }
        });

        $("#search").focus();

    }
});


// Email button create
$(document).ready(function () {

    var isOpenCreate = false;

    $("#emailCreateButton").click(function () {
        if (isOpenCreate) {
            $("#emailCreate").slideUp();
            isOpenCreate ^= true;
        }
        else {
            $("#emailCreate").slideDown();
            isOpenCreate ^= true;
        }
    });

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

$('input[name = "isInfluencerExternalLogin"]').change(function () {
    if ($('input[name = "isInfluencerExternalLogin"]:checked').val() === "true") {
        $("#isInfluencerExternalLoginBox").slideDown(300);
    }
    else {
        $("#isInfluencerExternalLoginBox").slideUp(300);
    }
});


// Change color if root or footer
$(document).ready(function () {
    var is_root = location.pathname == "/";

    if (!is_root) {
        $(".navbar-brand").css("color", "#FFA500");
    } else {
        $("#navbarLogo").attr("src", "/images/bestfluence-logo-white.svg");
    }

}); 