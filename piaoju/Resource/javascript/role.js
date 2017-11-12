//查询参数
var queryparam = {
    page: 0
}

var commitType = "add";//提交方法
var selectedTr = null;//编辑或删除的行

$(document).ready(function () {
    if ($("#hfLoginer").val() != "admin") {
        $("#btnShowAddRole").hide();
    }
    initQuery();
});

function initQuery() {
    $("#roletable").datagrid({
        url: '/Index/Roles',
        data: queryparam,
        showpagin: true
    });
}

function initShowAlert() {
    $(document).delegate(".showaction", "click", function () {
        var actiontype = $(this).attr("data-action");
        $("#btnCommit").attr("data-action", actiontype);
        if (actiontype == "add") {
            $("#roleno").val("");
            $("#rolename").val("");
            $("#memo").val("");

            $("#alertwindow").modal('show');
        }
        else {
            $.post("/Role/Single", { roleno: $(this).attr("data-id") }, function (rsp) {
                if (rsp && rsp.success === true) {
                    $("#roleno").val(rsp.data.roleno);
                    $("#rolename").val(rsp.data.rolename);
                    $("#memo").val(rsp.data.memo);

                    $("#alertwindow").modal('show');
                }
                else {
                    if (rsp) {
                        alert(decodeURIComponent(rsp.msg));
                    }
                    else {
                        alert("获取失败");
                    }
                }
            }, "json");
        }
    });
}

function initCommit(actiontype) {
    $("#btnCommit").click(function () {
        var actiontype = $(this).attr("data-action");
        var roleno = $("#roleno").val();
        var rolename = encodeURIComponent($("#rolename").val());
        var memo = encodeURIComponent($("#memo").val());

        var _this = this;
        $.post("/role/AddOrUpdate", {
            actiontype: actiontype,
            roleno: roleno,
            rolename: rolename,
            memo: memo
        }, function (rsp) {
            if (rsp && rsp.success === true) {
                if (actiontype == "add") {
                    var $tr = buildTr(rsp.data, "add");
                    $("#rolebody").prepend($tr);
                }
                else {
                    var $tr = $(".showaction[data-id='" + roleno + "']").parent().parent();
                    $tr.html(buildTr(rsp.data, "edit")).addClass("success");
                }
                $("#alertwindow").modal('hide');
            }
            else {
                if (rsp) {
                    alert(decodeURIComponent(rsp.msg));
                }
                else {
                    alert("提交失败");
                }
            }
        }, 'json');
    });
}

function initDelete() {
    $(document).delegate(".delete", "click", function () {
        if (!confirm("确认要删除吗")) {
            return;
        }
        var roleno = $(this).attr("data-id");
        var _this = this;
        $.post("/Role/Delete", { roleno: roleno }, function (rsp) {
            if (rsp && rsp.success === true) {
                $(_this).parent().parent().remove();
            }
            else {
                if (rsp) {
                    alert(decodeURIComponent(rsp.msg));
                }
                else {
                    alert("删除失败");
                }
            }
        }, 'json');
    });
}

function buildTr(data, action) {
    var html = "<td>" + data.rolename + "</td>\
                <td>"+ data.memo + "</td>\
                <td style='text-align:center;'>" + showEditButton(data, 1, data.roleno) + "</td>\
                <td style='text-align:center;'>" + showDeleteButton(data, 1, data.roleno) + "</td>";
    if (action == "add") {
        html = "<tr class='success'>" + html + "</tr>";
    }
    return html;
}

function showPowerButton(row, index, value) {
    var html = "";
    if ($("#hfLoginer").val() == "admin") {
        html = '<a style="cursor:pointer;" data-action="power" class="showaction" data-id="' + value + '"><i class="glyphicon glyphicon-cog" style="margin-right:0px; margin-top:3px;"></i></a>';
    }
    return html;
}

function showEditButton(row, index, value) {
    var html = "";
    if ($("#hfLoginer").val() == "admin") {
        html = '<a style="cursor:pointer;" data-action="edit" class="showaction" data-id="' + value + '"><i class="glyphicon glyphicon-edit" style="margin-right:0px; margin-top:3px;"></i></a>';
    }
    return html;
}

function showDeleteButton(row, index, value) {
    var html = "";
    if ($("#hfLoginer").val() == "admin") {
        html = '<a style="cursor:pointer;" class="delete" data-id="' + value + '"><i class="glyphicon glyphicon-trash" style="margin-right:0px; margin-top:3px;"></i></a>';
    }
    return html;
}