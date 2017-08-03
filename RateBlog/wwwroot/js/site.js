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

    YoutubeInput.keyup(function () {
        YoutubeText.text("www.youtube.com/user/" + YoutubeInput.val());
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
        $(this).prevAll().css({ "color": "#DCDCDC" });

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



// Scroll top button

window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 300 || document.documentElement.scrollTop > 300) {
        document.getElementById("topBtn").style.display = "block";
    } else {
        document.getElementById("topBtn").style.display = "none";
    }
}


function topFunction() {
    $('html, body').animate({
        scrollTop: 0
    }, 300);
}


// Ajax til search forslag

$(document).ready(function () {

    //setup before functions
    var typingTimer;                //timer identifier
    var doneTypingInterval = 500;  //time in ms, 5 second for example
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

