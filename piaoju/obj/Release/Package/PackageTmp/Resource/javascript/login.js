$(function () {
    $(".btn").click(function () {
        try {
            var userno = $("#username").val();
            var pwd = $("#password").val();

            if (userno == "" || pwd == "") {
                throw "用户名或密码不能为空";
            }

            var me = this;
            $(me).prop("disabled", true).find(".text").text("登陆中...");
            $.post('/Login/Index', { userno: userno, password: pwd,rnd:Math.random() }, function (rsp) {
                if (rsp && rsp.success === true) {
                    window.location = '/Index';
                }
                else {
                    if (rsp) {
                        alert(decodeURIComponent(rsp.msg));
                    } else {
                        alert('登陆失败');
                    }
                    $(me).prop("disabled", false).find(".text").text("登陆");
                }
            }, 'json').error(function () {
                alert(arguments[2]);
                $(me).prop("disabled", false).find(".text").text("登陆");
            }).complete(function () {
                
            });
        } catch (e) {
            alert(e);
        }
    });
});