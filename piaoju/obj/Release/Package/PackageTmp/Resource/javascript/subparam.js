function initAddOrUpdate() {
    $("#btnAdd").click(function () {
        var subno = $("#subno").val();
        var subname = $("#subname").val();

        $.post("/Affice/AddSub", { paramno: paramno, subno: subno, subname: subname, seq: 1 }, function (rsp) {
            if (rsp && rsp.success === true) {
                window.location = "/Affice/SubParams/" + paramno;
            }
            else {
                if (rsp) {
                    alert(decodeURIComponent(rsp.msg));
                }
                else {
                    alert("添加子项失败");
                }
            }
        }, "json");
    });

    $("#btnUpdate").click(function () {
        var subno = $("#subno").val();
        var subname = $("#subname").val();

        $.post("/Affice/UpdateSub", { paramno: paramno, subno: subno, subname: subname, seq: seq }, function (rsp) {
            if (rsp && rsp.success === true) {
                window.location = "/Affice/SubParams/" + paramno;
            }
            else {
                if (rsp) {
                    alert(decodeURIComponent(rsp.msg));
                }
                else {
                    alert("添加子项失败");
                }
            }
        }, "json");
    });
}

function initDeleteSub() {
    $(document).delegate(".delete", "click", function () {
        if (!confirm("确定要删除吗？")) {
            return;
        }

        var me = this;
        var id = $(me).attr("data-key");
        $.post("/Affice/DeleteSub", { id: id }, function (rsp) {
            if (rsp && rsp.success === true) {
                window.location = window.location;
            }
            else {
                if (rsp) {
                    alert(decodeURIComponent(rsp));
                }
                else {
                    alert("删除失败");
                }
            }
        }, "json");
    });
}