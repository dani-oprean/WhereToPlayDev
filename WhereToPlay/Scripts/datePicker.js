﻿$(function ()
{
    $('.date-picker').datepicker({ dateFormat: 'dd-mm-yy', 
        changeYear: true, 
        inline: true,
        showOtherMonths: true,
        dayNamesMin: ['Du', 'Lu', 'Ma', 'Mi', 'Jo', 'Vi', 'Sa'],
        nextText: '&rarr;',
        dateFormat: "dd.MM.yy",
        onSelect: function (date) {
        }
    });
})