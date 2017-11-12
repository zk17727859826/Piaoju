$(document).ready(function () {
    initSearchButton();
    initMoney();
    initSave();

    if ($("#xueyh").val() != "") {
        $("#btnGet").trigger("click");
    }
});

function initSearchButton() {
    $("#btnGet").click(function () {
        var xueyh = $("#xueyh").val();
        var shenfhm = $("#shenfhm").val();

        $.post('/Affice/xueyuan', { xueyh: xueyh, shenfhm: shenfhm }, function (rsp) {
            if (rsp && rsp.success === true) {
                $("#baomd").val(rsp.data.Baomd);
                $("#shenfhm").val(rsp.data.Shenfhm);
                $("#xueyh").val(rsp.data.Xueyh);
                $("#man").val(rsp.data.Xingming);
            }
            else {
                $("#baomd").val("");
                $("#shenfhm").val("");
                $("#xueyh").val("");
                $("#man").val("");
                if (rsp) {
                    alert(decodeURIComponent(rsp.msg));
                }
                else {
                    alert("学员不存在或获取失败");
                }
            }
        }, "json");
    });
}

function initMoney() {
    $("#money").blur(function () {
        var moneyvalue = $(this).val();
        $.post("/Affice/rmb", { money: moneyvalue }, function (rsp) {
            if (rsp && rsp.success === true) {
                $("#moneyUpper").text(rsp.money);
            }
            else {
                if (rsp) {
                    alert(decodeURIComponent(rsp.msg));
                }
                else {
                    alert("转换成大写失败");
                }
            }
        }, "json");
    });
}

function initSave() {
    $("#btnsave").click(function () {
        try {
            var bianma = $("#bianma").val();
            var xueyh = $("#xueyh").val();
            var baomd = $("#baomd").find("option:selected").text();
            var type = $("#type").val();
            var man = $("#man").val();
            var shoukfs = $("#shoukfs").val();
            var money = $("#money").val();
            var moneyUpper = $("#moneyUpper").text();
            var memo = $("#memo").val();

            if (bianma == "") {
                throw "编码不能为空";
            }
            if (baomd == "") {
                throw "请选择报名点";
            }
            if (type == "") {
                throw "请选择款项内容";
            }
            if (man == "") {
                throw "缴款单位或个人不能为空";
            }
            if (shoukfs == "") {
                throw "收款方式不能为空";
            }
            if (money == "") {
                throw "金额不能为空";
            }
            if (moneyUpper == "") {
                throw "人民币不能为空";
            }

            $.post("/Affice/Add", {
                bianm: bianma,
                xueyh: xueyh,
                baomd: baomd,
                type: type,
                man: man,
                shoukfs: shoukfs,
                money: money,
                moneyUpper: moneyUpper,
                memo: memo
            }, function (rsp) {
                if (rsp && rsp.success === true) {
                    alert("保存成功！");
                    $("#btnsave").remove();
                    //$("#btnprint").show();
                    window.location = '/Affice/Detail?id=' + bianma + "&back=accept";
                }
                else {
                    if (rsp) {
                        alert(decodeURIComponent(rsp.msg));
                    }
                    else {
                        alert("保存失败");
                    }
                }
            }, "json");
        } catch (e) {
            alert(e);
        }
    });
}