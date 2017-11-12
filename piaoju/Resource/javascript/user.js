//查询参数
var queryparam = {
    userno: "",
    username: "",
    page: 0
}

var commitType = "add";//提交方法
var selectedTr = null;//编辑或删除的行

$(document).ready(function () {
    initSearchButton();
    initShowAdd();
    initShowEdit();
    initDelete();
    initCommit();
});

function initShowAdd() {
    $("#btnShowAdd").click(function () {
        commitType = "add";
        $("#actiontype").text("添加");
        initShowAlert();
    });
}

function initShowEdit() {
    $(document).delegate(".edit", "click", function () {
        selectedTr = $(this).parent().parent();
        commitType = "edit";
        $("#actiontype").text("编辑");
        initShowAlert($(this).attr("data-id"));
    });
}

function initDelete() {
    $(document).delegate(".delete", "click", function () {
        if (confirm("确认删除此用户吗？")) {
            var _this = this;
            var id = $(this).attr("data-id");
            selectedTr = $(this).parent().parent();
            $.post("/Index/DeleteUser", { id: id }, function (rsp) {
                if (rsp && rsp.success) {
                    $(_this).parent().parent().remove();
                }
                else {
                    if (rsp) {
                        alert(rsp.msg);
                    }
                    else {
                        alert("删除失败");
                    }
                }
            }, 'json');
        }
    });
}

function initSearchButton() {
    $("#btnQuery").click(function () {
        var userno = $("#search_no").val();
        var username = $("#search_name").val();
        queryparam.userno = userno;
        queryparam.username = username;
        queryparam.page = 1;

        $("#usertable").datagrid({
            url: '/Index/Users',
            data: queryparam,
            showpagin: false
        });
    }).trigger("click");
}

function initShowAlert(id) {
    if (commitType == "add") {
        $("#bianm").val("").prop("disabled", false);
        $("#xingming").val("");
        $(":radio[name='sex']").prop("checked", false);
        $(":radio[name='power']").prop("checked", false);
    }
    else {
        $.post("/Index/SUser", { id: id }, function (rsp) {
            if (rsp && rsp.success == true) {
                $("#bianm").val(rsp.data.bianm).prop("disabled", true);
                $("#xingming").val(rsp.data.xingming);
                $(":radio[name='sex'][value='" + rsp.data.xingbie + "']").prop("checked", true);
                $(":radio[name='power'][value='" + rsp.data.power + "']").prop("checked", true);
                $("#alertwindow").modal("show");
            }
            else {
                if (rsp) {
                    alert(rsp.msg);
                }
                else {
                    alert("要编辑的用户不存在");
                }
                return;
            }
        }, "json").error(function () {
            alert(arguments[2]);
        });
    }
}

function initCommit() {
    $("#btnCommit").click(function () {
        var bianm = $.trim($("#bianm").val());
        var xingming = $("#xingming").val();
        var sex = $(":radio[name='sex']:checked").val();
        var power = $(":radio[name='power']:checked").val();
        var json = {
            bianm: bianm,
            xingming: xingming,
            sex: sex,
            power: power,
        };

        if (json.bianm == "") { alert("请输入用户代码"); return; }
        if (json.xingming == "") { alert("请输入用户姓名"); return; }
        if (json.sex == "" || typeof json.sex == "undefined") { alert("请选择性别"); return; }
        if (json.power == "" || typeof json.power == "undefined") { alert("请选择权限"); return; }

        if (commitType == "add") {
            json["actiontype"] = "add";
        }
        else {
            json["actiontype"] = "edit";
        }
        $(".btn-op").prop("disabled", true);
        $.post("/Index/UserAction", json, function (rsp) {
            if (rsp && rsp.success == true) {
                var html = '<td>' + rsp.data.bianm + '</td>\
                        <td>'+ rsp.data.xingming + '</td>\
                        <td style="width:50px; text-align:center;">'+ rsp.data.xingbie + '</td>\
                        <td style="width:50px; text-align:center;">' + rsp.data.power + '</td>\
                        <td style="width:50px; text-align:center;">' + showEditButton(rsp.data, 1, rsp.data.bianm) + '</td>\
                        <td style="width:50px; text-align:center;">' + showDeleteButton(rsp.data, 1, rsp.data.bianm) + '</td>';
                if (json.actiontype == "add") {
                    $("#userbody").prepend("<tr class='success'>" + html + "</tr>");
                    $('#alertwindow').modal('hide');
                }
                else if (json.actiontype == "edit") {
                    $(selectedTr).html(html).addClass("success");
                    $('#alertwindow').modal('hide');
                }
                else {
                    alert("操作类型失败");
                }
            }
            else {
                if (rsp) {
                    alert(rsp.msg);
                }
                else {
                    alert("提交失败");
                }
            }
        }, "json").complete(function () {
            $(".btn-op").prop("disabled", false);
        });
    });
}

function showEditButton(row, index, value) {
    var html = '<a style="cursor:pointer;" class="edit" data-id="' + value + '"><i class="glyphicon glyphicon-edit glyphicon-1x" style="margin-right:0px; margin-top:3px;"></i></a>';
    return html;
}

function showDeleteButton(row, index, value) {
    var html = "";
    html += '<a style="cursor:pointer;" class="delete" data-id="' + value + '"><i class="glyphicon glyphicon-trash glyphicon-1x" style="margin-right:0px; margin-top:3px;"></i></a>';
    return html;
}