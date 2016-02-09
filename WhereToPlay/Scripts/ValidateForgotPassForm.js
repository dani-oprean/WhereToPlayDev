$(document).ready(function () {
    var valid = false;//store email validity

    //verify email on blur
    $("[name='adresademail']").blur(function () {
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

            if (!$("#eroare").length) {
                $("#emailp").append("<span id='eroare' style='color:red;font-weight:bold;float:right;'>Adresa de email este invalida!</span>");
                $(this).attr("style", "border:2px solid red;");
            }
        }
        else {
            //if it's valid undo what you might have done if it wasn't valid
            $(this).attr("style", "");
            if ($("#eroare").length) $("#eroare").parent().children().remove("#eroare");
        }
    });

    //submit event
    $("#formulmeu").submit(function () {
        //if email is invalid go back and ask for an email, else if name is invalid do the same
        if (!valid) {
            $("[name='adresademail']").attr("style", "border:2px solid red;");
            if (!$("#eroare").length)
                $("#emailp").append("<span id='eroare' style='color:red;font-weight:bold;float:right;'>Adresa de email este invalida!</span>");
            return false;
        }
        return true;
    });
});