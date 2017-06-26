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
        data.Name = $("#opretNavn").val();
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
        WebsiteText = $("#editWebsiteText");

    if (FacebookInput.val() !== "") {
        FacebookText.text("www.facebook.com/" + $("#editFacebook").val());
    }

    if (InstagramInput.val() !== "") {
        InstagramText.text("www.instagram.com/" + InstagramInput.val());
    }

    if (YoutubeInput.val() !== "") {
        YoutubeText.text("www.youtube.com/user/" + YoutubeInput.val());
    }

    if (SnapchatInput.val() !== "") {
        SnapchatText.text("www.snapchat.com/add/" + SnapchatInput.val());
    }

    if (TwitterInput.val() !== "") {
        TwitterText.text("www.twitter.com/" + TwitterInput.val());
    }

    if (TwitchInput.val() !== "") {
        TwitchText.text("www.twitch.tv/" + TwitchInput.val()); 
    }

    if (WebsiteInput.val() !== "") {
        WebsiteText.text(WebsiteInput.val());
    }

    WebsiteInput.keyup(function () {
        WebsiteText.text(WebsiteInput.val());
    }); 

    TwitchInput.keyup(function () {
        TwitchText.text("www.twitch.tv/" + TwitchInput.val());
    });

    TwitterInput.keyup(function () {
        TwitterText.text("www.twitter.com/" + TwitterInput.val());
    });

    SnapchatInput.keyup(function () {
        SnapchatText.text("www.snapchat.com/add/" + SnapchatInput.val());
    });

    FacebookInput.keyup(function () {
        FacebookText.text("www.facebook.com/" + FacebookInput.val());
    });

    InstagramInput.keyup(function () {
        InstagramText.text("www.instagram.com/" + InstagramInput.val());
    });

}); 

//Rating Stars


$("label").click(function () {
    $(this).parent().find("label").css({ "background-color": "#D8D8D8" });
    $(this).css({ "background-color": "#7ED321" });
    $(this).nextAll().css({ "background-color": "#7ED321" });
});


$(document).ready(function () {
    $('[data-toggle="popover"]').popover();
});
    YoutubeInput.keyup(function () {
        YoutubeText.text("www.youtube.com/user/" + YoutubeInput.val());
    })


    // Select Category

    $("#selectCategory").select2();

}); 