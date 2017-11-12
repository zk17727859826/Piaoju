/*
* 对话框插件
*/
(function ($) {
    $.fn.extend({
        dialog: function (_config) {
            var _this = this;
            var config = {
                left: ($(window).width() - $(_this).width()) / 2,
                top: ($(window).height() - $(_this).height()) / 2,
                state: "open"
            };
            if (typeof (_config) == "object") {
                $.extend(config, _config);
            }
            else if (typeof (_config) == "function") {
                config.callback = _config;
            }
            else if (typeof (_config) == "string") {
                config.state = _config;
            }
            if (config.state == "open") {
                if ($(_this).attr("zx-dialog") != "1") {
                    $("body").append($(_this));
                    $(_this).attr("zx-dialog", "1");
                }
                $(_this).css({
                    position: "absolute",
                    left: config.left,
                    top: config.top
                }).show();

                if (typeof (config.callback) == "function") {
                    config.callback();
                }
            }
            else if (config.state == "close") {
                $(_this).hide();
            }
        },
        datagrid: function (_config, data) {
            var _this = this;
            var config = {};
            if ($(_this).attr("data-config")) {
                config = eval("(" + $(_this).attr("data-config") + ")");
            }
            $.extend(config, _config);
            var thlst = $(_this).find("thead > tr > th");
            if (thlst.size() == 0) {
                alert("请配置好表头");
                return;
            }

            var posturl = function () {
                if (config.trigger) {
                    $(config.trigger).attr("disabled", "disabled").text($(config.trigger).text() + "...");
                }
                config["rnd"] = Math.round();
                $.post(config.url, config.data || {}, function (rsp) {
                    if (rsp && rsp.success) {
                        var bodyobj = $(_this).find("tbody");
                        bodyobj.html("");
                        if (rsp.data.length > 0) {
                            for (var i = 0; i < rsp.data.length; i++) {
                                var _tr = "<tr>";
                                for (var j = 0; j < thlst.size(); j++) {
                                    var thconfig = undefined;
                                    if ($(thlst[j]).attr("data-config")) {
                                        thconfig = eval("({" + $(thlst[j]).attr("data-config") + "})");
                                    }
                                    if (thconfig) {
                                        var _width = (thconfig.width || "").toString().replace('px', '');
                                        _width = _width == '' ? 'auto' : (_width + 'px');
                                        var _text = ""
                                        if (thconfig.formatter) {
                                            _text = thconfig.formatter(rsp.data[i], i, rsp.data[i][thconfig.field]);
                                        }
                                        else if (typeof (thconfig.format) == "string") {
                                            var fields = thconfig.field.split(',');
                                            var _textstr = thconfig.format;
                                            for (var m = 0; m < fields.length; m++) {
                                                _textstr = _textstr.replace('{' + m + '}', rsp.data[i][fields[m]] == 0 ? "0" : (rsp.data[i][fields[m]] || ""));
                                            }
                                            _text = _textstr;
                                        }
                                        else {
                                            _text = rsp.data[i][thconfig.field] || '';
                                        }
                                        _tr += '<td style="text-align:' + (thconfig.align || 'left') + ';width:' + _width + '; vertical-align:' + (thconfig.valign || 'middle') + '">' + _text + '</td>';
                                    }
                                    else {
                                        _tr += '<td></td>';
                                    }
                                }
                                _tr += "</tr>";
                                bodyobj.append(_tr);
                            }
                            if (config.showfooter) {
                                bodyobj.parent().find("tfoot > tr > td").html(rsp.footertext);
                            }
                            if (config.showpagin) {
                                var firstenabled = rsp.page > 1 ? "" : "disabled";
                                var preenabled = rsp.page > 1 ? "" : "disabled";
                                var nextenabled = rsp.totalpage > 1 && rsp.page < rsp.totalpage ? "" : "disabled";
                                var lastenabled = rsp.totalpage == rsp.page ? "disabled" : "";
                                var ___pagingid = $(_this).attr("id") + '_tablepagin';
                                var ___pagintext = '\
                                    <a data-config="first" class="__gridpage" ' + firstenabled + ' style="display:inline-block; cursor:pointer; text-align:center; width:50px; height:25px; line-height:25px; border:solid 1px #aaa; border-radius:3px;">首页</a>\
                                    <a data-config="pre" class="__gridpage" ' + preenabled + ' style="display:inline-block; cursor:pointer; text-align:center; width:50px; height:25px; line-height:25px; border:solid 1px #aaa; border-radius:3px;">前页</a>\
                                    <a style="display:inline-block; text-align:center; width:50px; height:25px; line-height:25px;">' + (rsp.page || 0) + '/' + (rsp.totalpage || 0) + '</a>\
                                    <a data-config="next" class="__gridpage" ' + nextenabled + ' style="display:inline-block; cursor:pointer; text-align:center; width:50px; height:25px; line-height:25px; border:solid 1px #aaa; border-radius:3px;">后页</a>\
                                    <a data-config="last" class="__gridpage" ' + lastenabled + ' style="display:inline-block; cursor:pointer; text-align:center; width:50px; height:25px; line-height:25px; border:solid 1px #aaa; border-radius:3px;">尾页</a>';
                                if ($("#" + ___pagingid).size() == 0) {
                                    bodyobj.parent().parent().append('<div id="' + ___pagingid + '" style="height:25px; text-align:right; margin-top:5px; width:auto;">' + ___pagintext + '</div>');
                                }
                                else {
                                    $("#" + ___pagingid).html(___pagintext);
                                }

                                $(".__gridpage").click(function () {
                                    var dataconfig = $(this).attr("data-config");
                                    switch (dataconfig) {
                                        case "first":
                                            config.data["page"] = 1;
                                            break;
                                        case "pre":
                                            config.data["page"] = rsp.page - 1;
                                            break;
                                        case "next":
                                            config.data["page"] = rsp.page + 1;
                                            break;
                                        case "last":
                                            config.data["page"] = rsp.totalpage;
                                            break;
                                    }
                                    posturl();
                                });
                            }
                        }
                        else {
                            bodyobj.html("<tr><td style=\"text-align:center;\" colspan=\"" + thlst.size() + "\">暂无数据</td></tr>");
                        }
                    }
                    else {
                        if (rsp) {
                            alert(rsp.message || rsp.msg);
                        }
                        else {
                            alert("载入数据失败");
                        }
                    }
                }, 'json').complete(function () {
                    if (typeof config.callback == "function") {
                        config.callback();
                    }
                    if (config.trigger) {
                        $(config.trigger).removeAttr("disabled").text($(config.trigger).text().replace("...", ""));
                    }
                }).error(function () {
                    alert(arguments[2]);
                });
            }

            if (config.url) {
                posturl();
            }


        },
        ul: function (_config, data) {
            var _this = this;
            var config = {};
            if ($(_this).attr("data-config")) {
                config = eval("({" + $(_this).attr("data-config") + "})");
            }
            $.extend(config, _config);
            if (config.url) {
                $.post(config.url, {}, function (rsp) {
                    if (rsp && rsp.success) {
                        var bodyobj = $(_this);
                        bodyobj.html("");
                        if (rsp.data.length > 0) {
                            for (var i = 0; i < rsp.data.length; i++) {
                                var _li = '<li>' + rsp.data[i][config.field] + '</li>';
                                bodyobj.append(_li);
                            }
                        }
                        else {
                            bodyobj.html("");
                        }
                    }
                    else {
                        if (rsp) {
                            alert(rsp.message);
                        }
                        else {
                            alert("载入数据失败");
                        }
                    }
                }, 'json');
            }
        }
    });
})($);

/*
* 把URL编码search字串转换成键值对对象
* @url 键值对对象
*/
function getHashtable(url) {
    var urlarr = url.split('&');
    var obj = new Object();
    for (var i = 0; i < urlarr.length; i++) {
        var tmpitem = urlarr[i].split('=');
        if (obj[tmpitem[0]] != null) {
            obj[tmpitem[0]] = obj[tmpitem[0]] + "," + tmpitem[1];
        }
        else {
            obj[tmpitem[0]] = tmpitem[1];
        }
    }
    return obj;
}

/*
 * 让数组可以进行indexOf操作
 * @return 如果有值则返回指定的值所在的索引，如果没有则返回-1
 */
Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};

/*
 * 让数组可以进行remove操作
 */
Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

/*
 * 日期类的格式化
 */
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(), //day 
        "h+": this.getHours(), //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
        "S": this.getMilliseconds() //millisecond 
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}