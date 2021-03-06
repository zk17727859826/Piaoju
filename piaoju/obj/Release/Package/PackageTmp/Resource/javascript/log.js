﻿//查询参数
var queryparam = {
    startdate: "",
    enddate: "",
    oper: "",
    page: 0
}

$(document).ready(function () {
    initSearchButton();
});

function initSearchButton() {
    $("#btnQuery").click(function () {
        var startdate = $("#startdate").val();
        var enddate = $("#enddate").val();
        var oper = $("#oper").val();
        queryparam.startdate = startdate;
        queryparam.enddate = enddate;
        queryparam.oper = oper;
        queryparam.page = 1;

        $("#feetable").datagrid({
            url: '/Affice/log',
            data: queryparam,
            showpagin: true
        });
    }).trigger("click");
}

function showDetailsButton(row, index, value) {
    return "<a href='/Affice/Detail/" + value + "'>" + value + "</a>";
}

function showdate(row, index, value) {
    var birthdayMilliseconds = parseInt(obj.Birthday.replace(/\D/igm, ""));
    var _date = new Date(birthdayMilliseconds);
    return _date.format("yyyy/MM/dd HH:mm:ss");
}