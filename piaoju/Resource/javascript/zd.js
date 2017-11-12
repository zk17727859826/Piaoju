function initAdd() {
    $("#btnCommit").click(function () {
        var id = $("#id").val();
        var upid = $("#upid").val();
        var content = $("#content").val();
        var memo = $("#memo").val();

        if (content == "") {
            alert("请内容字典内容");
            return;
        }

        var optype = $(this).attr("data-optype");
        var url = "";
        if (optype == "add") {
            url = "/Affice/AddZidian1";
        }
        else {
            url = "/Affice/UpdateZidian1";
        }

        $.post(url, { id: id, upid: upid }, function (rsp) {
            if (rsp && rsp.success === true) {
                alert("提交成功")
                window.location = "/Affice/Zidian1?id=" + upid;
            }
            else {
                if (rsp) {
                    alert(decodeURIComponent(rsp.msg));
                }
                else {
                    alert("提交失败");
                }
            }
        }, "json");
    });
}

function initDelete() {
    $(".delete").click(function () {
        if (confirm("确定要删除吗")) {
            var id = $(this).attr("data-id");
            var upid = $(this).attr("data-upid");
            var url = "/Affice/DeleteZidian1";
            var _this = this;
            $.post(url, { id: id, upid: upid }, function (rsp) {
                if (rsp && rsp.success === true) {
                    alert("删除成功")
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
            }, "json");
        }
    });
}