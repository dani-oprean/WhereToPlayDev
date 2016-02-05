$(document).ready(function () {
    var valid = false;//store email validity
    var validName = false;//store name validity
    var kids = $("#menu").children("li");
    //hide word count
    $("[name='wordCount']").hide();
    $("[name='your_wordCount']").attr("readonly", true);

    //function set current page in jquery
    for (var i = 0; i < kids.length; i++) {
        var link = $(kids[i]).children("a").eq(0);
        if (link[0].pathname == $(location).attr('pathname')) {
            $(kids[i]).addClass("current");
        }
    }

    //verify email on blur
    $("[name='your_email']").blur(function () {
        //check email with regular expression
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        valid = re.test($(this).val());

        if (!valid) {
            //if it's not valid create a span with a message and make the border red
            $(this).attr("style", "border:2px solid red;");

            if ($(this).val().length > 0) $(this).focus();
            else {
                $(this).attr("style", "");
                if ($("#eroare").length) $("#eroare").parent().children().remove("#eroare");
            }

            if (!$("#eroare").length)
                $("#emailp").append("<span id='eroare' style='float:right;'>Email address is invalid!</span>");
        }
        else {
            //if it's valid undo what you might have done if it wasn't valid
            $(this).attr("style", "");
            if ($("#eroare").length) $("#eroare").parent().children().remove("#eroare");
        }
    });

    //verify name on blur
    $("[name='your_name']").blur(function () {
        //check name length
        validName = $("[name='your_name']").val().trim().length >= 1;

        if (!validName) {
            //if it's not valid create a span with a message and make the border red
            $(this).attr("style", "border:2px solid red;");

            if (!$("#eroareName").length)
                $("#namep").append("<span id='eroareName' style='float:right;'>You must have a name!</span>");
        }
        else {
            //if it's valid undo what you might have done if it wasn't valid
            $(this).attr("style", "");
            if ($("#eroareName").length) $("#eroareName").parent().children().remove("#eroareName");
        }
    });

    //word count for the textarea on key up 
    $("[name='your_enquiry']").keyup(function () {
        //display the word count input
        if ($("[name='wordCount']").css("display") == "none") $("[name='wordCount']").show();
        //count words
        var cuv = $(this).val().replace(/[.,-\/#!$%\^&\*;:{}=\-_`~()]/g, "").match(/\S+/g);

        //if there are words write the number in the word count input, else hide it
        if (cuv != null) $("[name='your_wordCount']").val(cuv.length.toString());
        else {
            $("[name='your_wordCount']").val('0');
            $("[name='wordCount']").hide();
        }
    });

    //submit event
    $("#formulmeu").submit(function () {
        //if email is invalid go back and ask for an email, else if name is invalid do the same
        if (!valid) {
            $("[name='your_email']").attr("style", "border:2px solid red;");
            if (!$("#eroare").length)
                $("#emailp").append("<span id='eroare' style='float:right;'>Email address is invalid!</span>");
            return false;
        } else if (!validName) {
            $("[name='your_name']").attr("style", "border:2px solid red;");
            if (!$("#eroareName").length)
                $("#namep").append("<span id='eroareName' style='float:right;'>You must have a name!</span>");
            return false;
        }
        return true;
    });
});