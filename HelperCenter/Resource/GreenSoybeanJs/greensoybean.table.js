(function($){
    Greensoybean.table = function (config) {
        var __gsb_table_object = $(config.selector).attr("gsb-table-object");
        $("#gsb_table_object_" + __gsb_table_object).remove();

        var __rnd_id = Math.random().toString().replace(".", "");
        $(config.selector).attr("gsb-table-object", __rnd_id);
        $(config.selector).find(".gsb-table-checkbox-all").remove();//如果有选择框则把第一列的全选框取消

        var isinit = true; //是否是在初始化;
        var me = this;
        var _tblcontain = undefined;
        var _tblcontent = undefined;
        var _tblheader = undefined;
        var _tblfooter = undefined;
        var _paginationdiv = undefined;
        var thisdata = undefined;
        var meconfig = {
            url: "",//
            param: {
                page: 1,
                pagesize: 30
            },
            async: true,//是否异步调用，默认为true
            keyfield: "",//键值
            pagination: false,//是否分页
            fixedtop: false,//是否固定表头
            loading: {//加载图层的大小
                width: 50,
                height: 50
            }
        };
        var isinit = true;//是否是初始化

        meconfig = $.extend(true, meconfig, config);
        var ths = $(meconfig.selector).find("thead th");
        if (ths.size() == 0) {
            throw "没有配置表头";
        }

        if (meconfig.async) {//添加异步调用
            meconfig.param["rnd"] = Math.random();
        }

        //获得数据源
        this.getdata = function () {
            $.post(meconfig.url, meconfig.param, function (data, textStatus, xhr) {
                if (data && data.success === true) {
                    var thisdata = data.rows;//当前的数据源
                    var trs = [];
                    for (var rowindex = 0; rowindex < data.rows.length; rowindex++) {
                        var row = data.rows[rowindex];
                        var tr = document.createElement("tr");
                        var keyfieldarr = meconfig.keyfield.split(',');
                        var keyfieldvalue = [];
                        for (var keyindex = 0; keyindex < keyfieldarr.length; keyindex++) {
                            keyfieldvalue.push(row[keyfieldarr[keyindex]]);
                        }

                        tr.setAttribute("data-key", keyfieldvalue.join("-"));//为tr设置键值
                        for (var colindex = 0; colindex < ths.length; colindex++) {
                            var thconfig = eval("({" + $(ths[colindex]).attr("data-config") + "})");
                            var td = document.createElement("td");
                            tr.appendChild(td);
                            var tdvalue = "";
                            if (thconfig.tdclass) {
                                td.className = thconfig.tdclass;
                            }
                            if (thconfig.field == "__ROW_INDEX") {
                                tdvalue = ((data.page - 1) * meconfig.param.pagesize) + rowindex + 1;
                            }
                            else {
                                tdvalue = row[thconfig.field];
                            }
                            if (thconfig.format) {
                                tdvalue = thconfig.format;
                                var datafields = thconfig.field.split(',');
                                for (var i = 0; i < datafields.length; i++) {
                                    tdvalue = tdvalue.replace("{" + i + "}", row[datafields[i]]);
                                }
                            }
                            else if (thconfig.formatter) {
                                if (typeof thconfig.formatter == "function") {
                                    tdvalue = thconfig.formatter(row, colindex, row[thconfig.keyfield]);
                                }
                            }
                            if (thconfig.align) {
                                td.style.textAlign = thconfig.align;
                                if (rowindex == 0) {
                                    ths[colindex].style.textAlign = thconfig.align;
                                }
                            }
                            if (thconfig.width) {
                                if (rowindex == 0) {
                                    ths[colindex].style.width = thconfig.width + "px";
                                }
                                td.style.width = thconfig.width + "px";
                                var _ellidiv = document.createElement("div");
                                _ellidiv.style.textOverflow = "ellipsis";
                                _ellidiv.style.width = thconfig.width + "px";
                                _ellidiv.style.overflow = "hidden";
                                _ellidiv.title = tdvalue;
                                _ellidiv.innerHTML = tdvalue;
                                td.appendChild(_ellidiv);
                            }
                            else {
                                td.innerHTML = tdvalue;
                            }
                            if (thconfig.checkbox === true) {
                                if (thconfig.showtext === false) {
                                    td.innerHTML = "";
                                }
                                var _checkbox = document.createElement("input");
                                _checkbox.type = "checkbox";
                                _checkbox.setAttribute("data-rowindex", rowindex);
                                _checkbox.className = "gsb-table-checkbox-item";
                                $(td).prepend(_checkbox);
                                if (rowindex == 0) {
                                    if (isinit) {
                                        var _checkall = document.createElement("input");
                                        _checkall.type = "checkbox";
                                        _checkall.className = "gsb-table-checkbox-all";
                                        $(document).delegate(".gsb-table-checkbox-all", "click", function () {
                                            if (meconfig.fixedtop) {
                                                $(_tblcontent).find(".gsb-table-checkbox-item").prop("checked", $(this).prop("checked"))
                                            }
                                            else {
                                                $(this).parent().parent().parent().parent().find(".gsb-table-checkbox-item").prop("checked", $(this).prop("checked"))
                                            }
                                        });
                                        $(ths[colindex]).prepend(_checkall);
                                        $(document).delegate(".gsb-table-checkbox-item", "click", function () {
                                            var itemcount = $(this).parent().parent().parent().find(".gsb-table-checkbox-item").size();
                                            var itemCheckedCount = $(this).parent().parent().parent().find(".gsb-table-checkbox-item:checked").size();
                                            if (itemcount == itemCheckedCount) {
                                                if (meconfig.fixedtop) {
                                                    $(_tblheader).find(".gsb-table-checkbox-all").prop("checked", true);
                                                }
                                                else {
                                                    $(this).parent().parent().parent().parent().find(".gsb-table-checkbox-all").prop("checked", true);
                                                }
                                            }
                                            else {
                                                if (meconfig.fixedtop) {
                                                    $(_tblheader).find(".gsb-table-checkbox-all").prop("checked", false);
                                                }
                                                else {
                                                    $(this).parent().parent().parent().parent().find(".gsb-table-checkbox-all").prop("checked", false);
                                                }
                                            }
                                        });
                                    }
                                }

                            }
                        }
                        trs.push(tr);
                    }

                    isinit = false; //表格内容初始化后，把初始化状态变成否
                    var tbody = $(meconfig.selector).find("tbody");
                    if (tbody.size() == 0) {
                        tbody = document.createElement("tbody");
                        $(meconfig.selector)[0].appendChild(tbody);
                    }
                    else {
                        tbody = tbody[0];
                    }
                    tbody.innerHTML = "";
                    for (var i = 0; i < trs.length; i++) {
                        tbody.appendChild(trs[i]);
                    }

                    //固定表头
                    if (meconfig.fixedtop) {
                        $(meconfig.selector).hide();
                        if (typeof _tblheader == "undefined") {
                            _tblheader = document.createElement("div");
                            _tblheader.className = "gsb-fixedtable-header";
                        }

                        if (typeof _tblcontent == "undefined") {
                            _tblcontent = document.createElement("div");
                            _tblcontent.className = "gsb-fixedtable-content";
                        }

                        if (typeof _tblcontain == "undefined") {
                            _tblcontain = document.createElement("div");
                            _tblcontain.className = "gsb-fixedtable";
                            _tblcontain.id = "gsb_table_object_" + __rnd_id;
                            if (meconfig.width) {
                                _tblcontain.style.width = meconfig.width;
                            }

                            _tblcontain.appendChild(_tblcontent);
                            _tblcontain.appendChild(_tblheader);
                        }

                        var _clonetblcontent = $(meconfig.selector).clone();
                        _clonetblcontent.attr("id", "");
                        _clonetblcontent.show();
                        _tblcontent.innerHTML = "";
                        _tblcontent.style.height = meconfig.height + "px";
                        _tblcontent.style.borderBottom = _clonetblcontent.css("border-bottom");
                        _tblcontent.appendChild(_clonetblcontent[0]);

                        $(meconfig.selector).before(_tblcontain);
                        var _clonetblheader = $(meconfig.selector).clone();
                        _clonetblheader.attr("id", "");
                        _clonetblheader.show();
                        _tblheader.innerHTML = "";
                        _tblheader.appendChild(_clonetblheader[0]);
                        _tblheader.style.height = _clonetblcontent.find("thead tr").height() + 1 + "px";
                        var _blankline = 2;
                        if (Greensoybean.version.firefox) {
                            _blankline = 0;
                        }
                        //加_blankline，是因为有的浏览器宽度不包括边框，所以要加上两边的边框
                        _clonetblheader.width(_clonetblcontent.width() + _blankline);
                    }

                    //分页
                    if (meconfig.pagination) {
                        var _total = data.total; //总记录数
                        var _page = meconfig.param.page; //当前页码
                        var _totalpage = Math.ceil(data.total / meconfig.param.pagesize);//总页码
                        var _firstdiv = document.createElement("a");
                        _firstdiv.innerHTML = "首页";
                        if (_page > 1) {
                            _firstdiv.className = "gsb-cursor-pointer";
                            _firstdiv.onclick = function () {
                                meconfig.param.page = 1;
                                me.getdata();
                            }
                        }

                        var _prediv = document.createElement("a");
                        _prediv.innerHTML = "前页";
                        if (_total > 0 && _page > 1) {
                            _prediv.className = "gsb-cursor-pointer";
                            _prediv.onclick = function () {
                                meconfig.param.page--;
                                if (meconfig.param.page < 1) {
                                    meconfig.param.page = 1;
                                }
                                me.getdata();
                            }
                        }

                        var _pagelabel = document.createElement("span");
                        _pagelabel.innerHTML = _page + " / " + _totalpage;

                        var _nextdiv = document.createElement("a");
                        _nextdiv.innerHTML = "后页";
                        if (_page < _totalpage) {
                            _nextdiv.className = "gsb-cursor-pointer";
                            _nextdiv.onclick = function () {
                                meconfig.param.page++;
                                if (meconfig.param.page > _totalpage) {
                                    meconfig.param.page = _totalpage;
                                }
                                me.getdata();
                            }
                        }

                        var _lastdiv = document.createElement("a");
                        _lastdiv.innerHTML = "尾页";
                        if (_page < _totalpage) {
                            _lastdiv.className = "gsb-cursor-pointer";
                            _lastdiv.onclick = function () {
                                meconfig.param.page = _totalpage;
                                me.getdata();
                            }
                        }

                        if (typeof _paginationdiv == "undefined") {
                            _paginationdiv = document.createElement("div");
                            _paginationdiv.className = "glb-pagination";
                            if (meconfig.fixedtop) {
                                _tblcontain.appendChild(_paginationdiv);
                            }
                            else {
                                $(meconfig.selector).next(".glb-pagination").remove();
                                $(meconfig.selector).after(_paginationdiv);
                                _paginationdiv.style.width = $(meconfig.selector).width() + "px";
                            }
                        }
                        _paginationdiv.innerHTML = "";
                        _firstdiv.className += " glb-pagination-btn glb-pagination-btn-first";
                        _paginationdiv.appendChild(_firstdiv);
                        _prediv.className += " glb-pagination-btn glb-pagination-btn-pre";
                        _paginationdiv.appendChild(_prediv);
                        _paginationdiv.appendChild(_pagelabel);
                        _nextdiv.className += " glb-pagination-btn glb-pagination-btn-next";
                        _paginationdiv.appendChild(_nextdiv);
                        _lastdiv.className += " glb-pagination-btn glb-pagination-btn-last";
                        _paginationdiv.appendChild(_lastdiv);
                    }
                }
                else {
                    if (data) {
                        alert(data.message || data.msg);
                    }
                    else {
                        alert("数据加载失败");
                    }
                }
            }, "json").error(function (){
                alert("抓取失败：" + arguments[2]);
            });
        }

        //执行获得数据源
        this.getdata();

        //获得选中状态的数据行
        this.getSelectedRows = function () {
            var _selectedrows = [];
            _tblcontent.find(".gsb_table_checked_item:checked").each(function () {
                _selectedrows.push(me.thisdata[$(this).attr("data-index")]);
            });
            return _selectedrows;
        }
    }
})(jQuery)