$(document).ready(function () {
    $("#btnCommit").click(function () {
        try {
            var oldpwd = $("#oldpassword").val();
            var newpwd = $("#newpassword").val();
            var rptpwd = $("#repeatpassword").val();

            if (oldpwd == "") {
                throw "原始密码不能为空！";
            }
            if (newpwd == "") {
                throw "新设密码不能为空！";
            }
            if (newpwd != rptpwd) {
                throw "两次输入的新密码不一致";
            }
            if (oldpwd == newpwd) {
                throw "原始密码与新设密码不能一致";
            }
            var me = this;
            $(me).prop("disabled", true);
            $.post("/Index/Password", {
                oldpwd: oldpwd, newpwd: newpwd
            }, function (rsp) {
                if (rsp && rsp.success === true) {
                    $("#oldpassword").val("");
                    $("#newpassword").val("");
                    $("#repeatpassword").val("");
                    alert("密码修改成功");
                }
                else {
                    if (rsp) {
                        alert(decodeURIComponent(rsp.msg));
                    }
                    else {
                        alert("密码修改失败");
                    }
                }
            }, "json").complete(function () {
                $(me).prop("disabled", false);
            });
        } catch (e) {
            alert(e);
        }
    });
});