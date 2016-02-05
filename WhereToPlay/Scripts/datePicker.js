$(function ()
{
    $('.date-picker').datepicker({ changeYear: true, 
        inline: true,
        showOtherMonths: true,
        dayNamesMin: ['Du', 'Lu', 'Ma', 'Mi', 'Jo', 'Vi', 'Sa'],
        monthNames: ['Ianuarie', 'Februarie', 'Martie', 'Aprilie', 'Mai', 'Iunie',
            'Iulie', 'August', 'Septembrie', 'Octombrie', 'Noiembrie', 'Decembrie'],
        monthNamesShort: ['Ian', 'Feb', 'Mar', 'Apr', 'Mai', 'Iun',
            'Iul', 'Aug', 'Sep', 'Oct', 'Noi', 'Dec'],
        nextText: '&rarr;',
        prevText: '&larr;',
        onSelect: function (date) {
            var dateResult = new Date(date);
            var resultDay = dateResult.getDate();
            var resultMonth = dateResult.getMonth() + 1;
            var resultYear = dateResult.getFullYear();
            var model = Model.IDCourt;

            $.get("/Courts/ShowTimes", { dateRes: resultDay+"."+resultMonth+"."+resultYear});
        }
    }).datepicker("setDate", new Date());
})

