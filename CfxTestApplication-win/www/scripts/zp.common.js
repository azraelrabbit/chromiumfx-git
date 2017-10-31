$.extend({
    zp: {
        getGuidItem: function () {
            return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
        },
        createId: function () {
            var ticks = ((new Date().getTime() * 10000) + 621355968000000000);
            return '0' +
                ticks +
                $.zp.getGuidItem() +
                $.zp.getGuidItem() +
                $.zp.getGuidItem() +
                $.zp.getGuidItem() +
                $.zp.getGuidItem() +
                $.zp.getGuidItem() +
                $.zp.getGuidItem() +
                $.zp.getGuidItem();
        },
        /**
         * @param {} len 
         * @returns {} 
         */
        getRndStr: function (len) {
            var s = [];
            var a = parseInt(Math.random() * 25) + (Math.random() > 0.5 ? 65 : 97);
            for (var i = 0; i < len; i++) {
                s[i] = Math.random() > 0.5
                    ? parseInt(Math.random() * 9)
                    : String.fromCharCode(parseInt(Math.random() * 25) + (Math.random() > 0.5 ? 65 : 97));
            }
            return s.join('');
        },

        /**
         * 
         * @param {} type 
         * @param {} content 
         * @param {} interval 
         * @param {} delaycallback 
         * @returns {} 
         * showPopAlert Example:
            $.zp.showPopAlert("i", "<strong>Information Title</strong><br>" + aircraftIds);
            $.zp.showPopAlert("S", "<strong>Success Title</strong><br>" + aircraftIds, 2000, popCallback);
            $.zp.showPopAlert("w", "<strong>Warning Title</strong><br>" + aircraftIds, 4000);
            $.zp.showPopAlert("D", "<strong>Danger Title</strong><br>Danger" + aircraftIds, 8000, popCallback);
         */
        alert: function (type, content, interval, delaycallback) {
            var t = $(".zp-pop-content")[0];
            if (t) {
                var id = $.zp.getRndStr(15);
                var style = "info";
                if (type.toLowerCase().match("^s")) {
                    style = "success";
                } else if (type.toLowerCase().match("^w")) {
                    style = "warning";
                } else if (type.toLowerCase().match("^d")) {
                    style = "danger";
                }
                var html = "" +
                    "<div id='" + id + "' class='alert alert-" + style + "'>" +
                    "   <button type='button' class='close' data-dismiss='alert' >" +
                    "       <span class='glyphicon glyphicon-remove'></span>" +
                    "   </button>" +
                    "   <div>" + content + "</div>" +
                    "</div>";

                $(t).append(html);

                var alert = $("#" + id);
                if (alert) {
                    if (delaycallback) {
                        $(alert).bind('closed.bs.alert', function () {
                            delaycallback(alert);
                        });
                    }

                    if (interval) {
                        setTimeout(function () {
                            alert.fadeOut();
                            if (delaycallback) {
                                delaycallback(alert);
                            }
                        }, interval);
                    }
                }
                return alert;
            } else {
                console.log("class .zp-pop-content not defined.");
                return false;
            }
        },
        alertSuccess: function (content, interval, delaycallback) {
            $.zp.alert('S', content, interval, delaycallback);
        },
        alertInfo: function (content, interval, delaycallback) {
            $.zp.alert('I', content, interval, delaycallback);
        },
        alertWarning: function (content, interval, delaycallback) {
            $.zp.alert('W', content, interval, delaycallback);
        },
        alertDanger: function (content, interval, delaycallback) {
            $.zp.alert('D', content, interval, delaycallback);
        },
        /**
         * 
         * @param {} type 
         * @param {} content 
         * @param {} buttons 
         * @param {} okayCallback 
         * @param {} cancelCallback 
         * @param {} closeCallback 
         * @param {} delaycallback 
         * @returns {} 
         */
        showPop: function (type, content, buttons, okayCallback, cancelCallback, closeCallback) {

            // confirm  glyphicon-question-sign
            // info     glyphicon-info-sign
            // succ     glyphicon-ok-sign
            // warning  fa-warning
            // danger    glyphicon-remove-sign
            var autoClose = false;
            var backgroundDismiss = false;
            var theme = "zp-warning";
            var icon = "glyphicon glyphicon-info-sign";
            var title = "Information";

            if (type.toLowerCase().match("^c")) {           // confirm  glyphicon-question-sign
                icon = "glyphicon glyphicon-question-sign";
                title = "Confirm";

                if (buttons) {
                    //
                } else {
                    buttons = {
                        okay: {
                            text: 'Okay',
                            btnClass: 'btn-primary',
                            action: function () {
                                if (okayCallback) {
                                    okayCallback();
                                }
                            }
                        },
                        cancel: {
                            text: 'Cancel',
                            btnClass: 'btn-default',
                            action: function () {
                                if (cancelCallback) {
                                    cancelCallback();
                                }
                            }
                        }
                    };
                }
                theme = "zp-confirm";

            } else if (type.toLowerCase().match("^i")) {    // info     glyphicon-info-sign
                icon = "glyphicon glyphicon-info-sign";
                title = "Information";

                if (buttons) {
                    //
                } else {
                    buttons = {
                        okay: {
                            text: 'Close',
                            btnClass: 'btn-info',
                            action: function () {
                                if (closeCallback) {
                                    closeCallback();
                                }
                            }
                        }
                    };
                    autoClose = "okay|5000";
                }
                backgroundDismiss = true;
                theme = "zp-info";

            } else if (type.toLowerCase().match("^s")) {    // succ     glyphicon-ok-sign
                icon = "glyphicon glyphicon-ok-sign";
                title = "Succeed";

                if (buttons) {
                    //
                } else {
                    buttons = {
                        okay: {
                            text: 'Close',
                            btnClass: 'btn-success',
                            action: function () {
                                if (closeCallback) {
                                    closeCallback();
                                }
                            }
                        }
                    };
                    autoClose = "okay|5000";
                }
                backgroundDismiss = true;
                theme = "zp-succ";
            } else if (type.toLowerCase().match("^w")) {    // warning  fa-warning
                icon = "fa fa-warning";
                title = "Warning";

                if (buttons) {
                    //
                } else {
                    buttons = {};
                    if (okayCallback) {
                        buttons.okay= {
                            text: 'OK',
                            btnClass: 'btn-warning',
                            action: function () {
                                if (okayCallback) {
                                    okayCallback();
                                }
                            }
                        };
                    }

                    buttons.cancel= {
                            text: 'Close',
                            btnClass: 'btn-warning',
                            action: function () {
                                if (closeCallback) {
                                    closeCallback();
                                }
                            }
                    };
                }
            } else if (type.toLowerCase().match("^d")) {    // fault    glyphicon-remove-sign
                icon = "glyphicon glyphicon-remove-sign";
                title = "Danger";

                if (buttons) {
                    //
                } else {
                    buttons = {
                        okay: {
                            text: 'Close',
                            btnClass: 'btn-danger',
                            action: function () {
                                if (closeCallback) {
                                    closeCallback();
                                }
                            }
                        }
                    };
                }
                theme = "zp-danger";
            }

            var option = {
                //template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="jconfirm-scrollpane"><div class="container"><div class="row"><div class="jconfirm-box-container"><div class="jconfirm-box" role="dialog" aria-labelledby="labelled" tabindex="-1"><div class="closeIcon">&times;</div><div class="title-c"><span class="icon-c"></span><span class="title"></span></div><div class="content-pane"><div class="content"></div></div><div class="buttons"></div><div class="jquery-clear"></div></div></div></div></div></div></div>',
                title: title,
                columnClass: 'large',
                content: content,
                //contentLoaded: function() {},
                icon: icon, //icon: 'fa fa-warning',
                buttons: buttons,
                confirmButton: 'Okay',
                cancelButton: 'Cancel',
                confirmButtonClass: 'btn-primary',
                cancelButtonClass: 'btn-default',
                theme: theme, //theme:'white', 'light','supervan' ,'dark', 'material', 'bootstrap'

                //theme: 'bootstrap', //theme:'white', 'light','supervan' ,'dark', 'material', 'bootstrap'
                //animation: 'zoom',
                //closeAnimation: 'scale',
                //animationSpeed: 500,
                //animationBounce: 1.2,
                //keyboardEnabled: false,
                //rtl: false,
                //confirmKeys: [13], // ENTER key
                //cancelKeys: [27], // ESC key
                //container: 'body',
                //confirm: function () {},
                //cancel: function () {},
                //backgroundDismiss: false,
                autoClose: autoClose, //autoClose: 'cancelAction|8000'
                backgroundDismiss: backgroundDismiss,
                //closeIcon: null,
                //closeIconClass: false,
                //watchInterval: 100,
                //columnClass: 'col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3 col-xs-10 col-xs-offset-1',
                onOpen: function () {
                },
                onClose: function () {
                },
                onAction: function () {
                }
            };

            $.confirm(option);
        },
        confirm: function (content, okayCallback, cancelCallback) {
            $.zp.showPop('c', content, null, okayCallback, cancelCallback);
        },
        /**
         * 
         * @param {string} type [c-confirm,i-info,s-success,w-warning,d-danger] 
         * @param {string} content
         * @param {function()} okayCallback
         * @param {function()} cancelCallback
         */
        confirmType: function (type, content, okayCallback, cancelCallback) {
            $.zp.showPop(type, content, null, okayCallback, cancelCallback);
        },
        confirmCustom: function (content, buttons) {
            $.zp.showPop('c', content, buttons);
        },
        /**
         * 
         * @param {string} type [c-confirm,i-info,s-success,w-warning,d-danger]
         * @param {string} content
         * @param {[]} buttons
         */
        confirmCustomType: function (type,content, buttons) {
            $.zp.showPop(type, content, buttons);
        },
        confirmInfo: function (content, closeCallback) {
            $.zp.showPop('i', content, null, null, null, closeCallback);
        },
        confirmSuccess: function (content, closeCallback) {
            $.zp.showPop('s', content, null, null, null, closeCallback);
        },
        confirmWarning: function (content, closeCallback) {
            $.zp.showPop('w', content, null, null, null, closeCallback);
        },
        confirmDanger: function (content, closeCallback) {
            $.zp.showPop('d', content, null, null, null, closeCallback);
        },
        /**
         * 
         * @param {} paramUrl 
         * @param {} paramData 
         * @param {} paramSuccess 
         * @param {} paramError 
         * @param {} paramType 
         * @param {} paramAsync 
         * @returns {} 
         */
        jsonAuthAjax: function jsonAuthAjax(paramUrl, paramData, paramSuccess, paramError, paramType, paramAsync) {
            if (arguments.length < 4) {
                paramError = function (response) {
                    console.log(response);
                };
                paramType = "GET";
                paramAsync = false;
            } else if (arguments.length < 5) {
                paramAsync = false;
            }

            if (!paramError) {
                paramError = function (response) {
                    console.log(response);
                };
            }

            $.ajax({
                cache: false,
                url: paramUrl,
                data: paramData,
                type: paramType,
                traditional: true,
                async: paramAsync,
                error: function (response) {
                    console.log(response);
                    paramError(response);
                },
                success: function (response) {
                    paramSuccess(response);
                }
            });
        },
        /**
         * 
         * @param {} paramUrl 
         * @param {} paramData 
         * @param {} onSuccess 
         * @param {} onError 
         * @param {} isasync 
         * @returns {} 
         */
        jsonAjaxPost: function (paramUrl, paramData, onSuccess, onError, isasync) {
            if (!onError) {
                onError = function (response) {
                    console.log(response);
                };
            }
            $.zp.jsonAuthAjax(paramUrl, paramData, onSuccess, onError, "POST", isasync);
        },
        /**
         * 
         * @param {} paramUrl 
         * @param {} paramData 
         * @param {} onSuccess 
         * @param {} onError 
         * @param {} isasync 
         * @returns {} 
         */
        jsonAjaxGet: function (paramUrl, paramData, onSuccess, onError, isasync) {
            if (!onError) {
                onError = function (response) {
                    console.log(response);
                };
            }
            $.zp.jsonAuthAjax(paramUrl, paramData, onSuccess, onError, "GET", isasync);
        },
        /**
         * 
         * @param {} array 
         * @param {} val 
         * @returns {} 
         */
        pushItemToArray: function (array, item, pushCallback, notOverwrite) {
            if (!array) {
                array = [];
            }
            if (notOverwrite) {
                array.push(item);
                if (pushCallback) {
                    pushCallback(array, item);
                }
            } else {
                var i = array.indexOf(item);
                if (i < 0) {
                    array.push(item);
                    if (pushCallback) {
                        pushCallback(array, item);
                    }
                }
            }
        },
        /**
         * 
         * @param {} array 
         * @param {} val 
         * @returns {} 
         */
        removeItemFromArray: function (array, item, removeCallback) {
            if (array) {
                var i = array.indexOf(item);
                if (i > -1) {
                    array.splice(i, 1);
                    if (removeCallback) {
                        removeCallback(array, item);
                    }
                }
            }
        },
        /**
         * 
         * @param {} controlId 
         * @param {} uploadUrl 
         * @param {} allowedFileExtensions 
         * @param {} fileUploadedCallback 
         * @param {} filePredeleteCallback 
         * @returns {} 
         */
        fileUpload: function (controlId, uploadUrl, allowedFileExtensions, fileUploadedCallback, filePredeleteCallback) {
            if ($.fn.fileinput) {
                var control = $('#' + controlId);
                if (control) {
                    control.fileinput({
                        language: 'en',
                        uploadUrl: uploadUrl,
                        uploadAsync: true,
                        showUpload: true,
                        showRemove: true,
                        browseClass: 'btn btn-primary',
                        previewFileIcon: '<i class="fa fa-file"></i>',
                        allowedFileExtensions: allowedFileExtensions,
                        allowedPreviewTypes: allowedFileExtensions,
                        previewFileIconSettings: {
                            'docx': '<i class="fa fa-file-word-o text-primary"></i>',
                            'xlsx': '<i class="fa fa-file-excel-o text-success"></i>',
                            'pptx': '<i class="fa fa-file-powerpoint-o text-danger"></i>',
                            'jpg': '<i class="fa fa-file-photo-o text-warning"></i>',
                            'pdf': '<i class="fa fa-file-pdf-o text-danger"></i>',
                            'zip': '<i class="fa fa-file-archive-o text-muted"></i>'
                        },
                        maxFileCount: 10,
                        enctype: 'multipart/form-data'
                    });

                    if (fileUploadedCallback) {
                        $(control).on("fileuploaded", function (event, data, previewId, index) {
                            fileUploadedCallback(event, data, previewId, index);
                        });
                    }

                    if (filePredeleteCallback) {
                        $(control).on("fileuploaded", function (event, data, previewId, index) {
                            filePredeleteCallback(event, data, previewId, index);
                        });
                    }
                }
            }
        },
        /**
         * 
         * @param {} message 
         * @param {} confirmCallback 
         * @param {} cancelCallback 
         * @returns {} 
         */
        //confirm: function (message, confirmCallback, cancelCallback) {
        //    $.confirm({
        //        title: 'Confirm',
        //        content: message,
        //        buttons: {
        //            ok: {
        //                btnClass: 'btn-danger',
        //                action: function () {
        //                    if (confirmCallback) {
        //                        confirmCallback();
        //                    }
        //                }
        //            },
        //            cancle: {
        //                btnClass: 'btn-warning',
        //                action: function () {
        //                    if (cancelCallback) {
        //                        cancelCallback();
        //                    }
        //                }
        //            }
        //        }
        //    });
        //},
        /**
         * 
         * @param {} value 
         * @param {} type 
         * @returns {} 
         */
        bootstrapTableDateTimeColumnFormatter: function (value, type) {
            var ret = '<div title="--TITLE--">--DATETIME--</div>';
            if (value) {
                if (value.toLowerCase().match("^\/d")) {
                    value = parseInt(value.slice(6, -2));
                }
                try {
                    var date = new Date(value);
                    var optionsTitle = {
                        hour12: false,
                        year: "numeric",
                        month: "2-digit",
                        day: "2-digit",
                        hour: "2-digit",
                        minute: "2-digit",
                        second: "2-digit",
                        weekday: "long"
                    };
                    var optionsShort = {
                        hour12: false,
                        year: "numeric",
                        month: "2-digit",
                        day: "2-digit"
                    };
                    var optionsLong = {
                        hour12: false,
                        year: "numeric",
                        month: "2-digit",
                        day: "2-digit",
                        hour: "2-digit",
                        minute: "2-digit",
                        second: "2-digit"
                    };
                    if (date > new Date(1903, 1, 1)) {
                        if (type.toLowerCase().match("^d")) {
                            ret = ret.replace("--TITLE--", date.toLocaleString('zh', optionsTitle)).replace("--DATETIME--", date.toLocaleString('zh', optionsShort));
                        } else {
                            ret = ret.replace("--TITLE--", date.toLocaleString('zh', optionsTitle)).replace("--DATETIME--", date.toLocaleString('zh', optionsLong));
                        }
                    } else {
                        ret = ret.replace("--TITLE--", "Error").replace("--DATETIME--", "-");
                    }

                } catch (error) {
                    console.log(value);
                    ret = ret.replace("--TITLE--", "Value Error:" + value).replace("--DATETIME--", "-");
                }
                return ret;
            }
            return ret;
        },
        /**
         * 
         * @param {} targetdiv
         * @param {} url
         * @param {} params
         * @param {boolean} async
         * @returns {} 
         */
        loadPartialView:function loadPartialView(targetdiv, url, params, async) {
            targetdiv.empty();
            targetdiv.addClass("loader");
            targetdiv.append("加载中... ...");
    
            //$.post(url, params)
            //    .success(function (e) {
            //        targetdiv.empty().append(e);
            //        targetdiv.removeClass("loader");
            //    });

            $.zp.jsonAjaxPost(url, params, function (response) {
                targetdiv.empty().append(response);
                targetdiv.removeClass("loader");
            }, null, async);
            return;
        }
    }
});

//function zpConfirm(message, confirmCallback, cancelCallback) {
//    $.confirm({
//        title: '操作确认',
//        content: message,
//        confirmButton: '确定',
//        cancelButton: '取消',
//        confirm: function () {
//            if (confirmCallback) {
//                confirmCallback();
//            }
//        },
//        cancel: function () {
//            if (cancelCallback) {
//                cancelCallback();
//            }
//        }
//    });
//}

function JSONToCSVConvertor(jsonData, reportTitle, showLabel) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof jsonData != 'object' ? JSON.parse(jsonData) : jsonData;

    var csvContent = '';
    //Set Report title in first row or line
    csvContent += reportTitle + '\r\n\n';

    //This condition will generate the Label/Header
    if (showLabel) {
        var row = "";

        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            row += index + ',';
        }
        row = row.slice(0, -1);
        //append Label row with line break
        csvContent += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }
        row.slice(0, row.length - 1);

        //add a line break after each row
        csvContent += row + '\r\n';
    }

    if (csvContent == '') {
        // alert("Invalid data");
        slog("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += reportTitle.replace(/ /g, "_");

    if (isIE()) {
        var blob = new Blob([decodeURIComponent(encodeURI("\uFEFF" + csvContent))], {
            type: "text/csv;charset=utf-8;"
        });
        navigator.msSaveBlob(blob, fileName + ".csv");
    } else {
        //Initialize file format you want csv or xls
        var uri = 'data:text/csv;charset=utf-8,\uFEFF' + encodeURI(csvContent);

        // Now the little tricky part.
        // you can use either>> window.open(uri);
        // but this will not work in some browsers
        // or you will not get the correct file extension    

        //this trick will generate a temp <a /> tag
        var link = document.createElement("a");
        link.href = uri;

        //set the visibility hidden so it will not effect on your web-layout
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";

        //this part will append the anchor tag and remove it after automatic click
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}
function isIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return version number 
    {
        return true;
    } else { // If another browser, 
        return false;
    }
    return false;
}


//function date2Str(x, y) {
//    var z = { M: x.getMonth() + 1, d: x.getDate(), h: x.getHours(), m: x.getMinutes(), s: x.getSeconds() };
//    y = y.replace(/(M+|d+|h+|m+|s+)/g, function (v) { return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1))).slice(-2); });
//    return y.replace(/(y+)/g, function (v) { return x.getFullYear().toString().slice(-v.length); });
//}

//function ddDays(date, days) {
//    var nd = new Date(date);
//    nd = nd.valueOf();
//    nd = nd + days * 24 * 60 * 60 * 1000;
//    nd = new Date(nd);
//    //alert(nd.getFullYear() + "年" + (nd.getMonth() + 1) + "月" + nd.getDate() + "日");
//    var y = nd.getFullYear();
//    var m = nd.getMonth() + 1;
//    var d = nd.getDate();
//    if (m <= 9) m = "0" + m;
//    if (d <= 9) d = "0" + d;
//    var cdate = y + "-" + m + "-" + d;
//    return cdate;
//}

//function addDaysDate(date, days) {
//    var nd = new Date(date);
//    nd = nd.valueOf();
//    nd = nd + days * 24 * 60 * 60 * 1000;
//    nd = new Date(nd);
//    //alert(nd.getFullYear() + "年" + (nd.getMonth() + 1) + "月" + nd.getDate() + "日");
//    var y = nd.getFullYear();
//    var m = nd.getMonth() + 1;
//    var d = nd.getDate();
//    if (m <= 9) m = "0" + m;
//    if (d <= 9) d = "0" + d;
//    var cdate = y + "-" + m + "-" + d;
//    return new Date(cdate);
//}

//Date.prototype.format = function (format) {
//    var o = {
//        "M+": this.getMonth() + 1, //month
//        "d+": this.getDate(), //day
//        "h+": this.getHours(), //hour
//        "m+": this.getMinutes(), //minute
//        "s+": this.getSeconds(), //second
//        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
//        "S": this.getMilliseconds() //millisecond
//    }
//    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
//    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
//    for (var k in o) if (new RegExp("(" + k + ")").test(format))
//        format = format.replace(RegExp.$1,
//        RegExp.$1.length == 1 ? o[k] :
//        ("00" + o[k]).substr(("" + o[k]).length));
//    return format;
//}

//String.prototype.PadLeft = function (totalWidth, paddingChar) {
//    if (paddingChar != null) {
//        return this.PadHelper(totalWidth, paddingChar, false);
//    } else {
//        return this.PadHelper(totalWidth, ' ', false);
//    }
//}
//String.prototype.PadRight = function (totalWidth, paddingChar) {
//    if (paddingChar != null) {
//        return this.PadHelper(totalWidth, paddingChar, true);
//    } else {
//        return this.PadHelper(totalWidth, ' ', true);
//    }

//}
//String.prototype.PadHelper = function (totalWidth, paddingChar, isRightPadded) {

//    if (this.length < totalWidth) {
//        var paddingString = new String();
//        for (i = 1; i <= (totalWidth - this.length) ; i++) {
//            paddingString += paddingChar;
//        }

//        if (isRightPadded) {
//            return (this + paddingString);
//        } else {
//            return (paddingString + this);
//        }
//    } else {
//        return this;
//    }
//}

function flashChecker() {
    var hasFlash = 0;　　　　 //是否安装了flash 
    var flashVersion = 0;　　 //flash版本 
    if (document.all) {
        var swf = new ActiveXObject('ShockwaveFlash.ShockwaveFlash');
        if (swf) {
            hasFlash = 1;
            VSwf = swf.GetVariable("$version");
            flashVersion = parseInt(VSwf.split(" ")[1].split(",")[0]);
        }
    } else {
        if (navigator.plugins && navigator.plugins.length > 0) {
            var swf = navigator.plugins["Shockwave Flash"];
            if (swf) {
                hasFlash = 1;
                var words = swf.description.split(" ");
                for (var i = 0; i < words.length; ++i) {
                    if (isNaN(parseInt(words[i]))) continue;
                    flashVersion = parseInt(words[i]);
                }
            }
        }
    }
    return {
        f: hasFlash,
        v: flashVersion
    };
}

var ZP = { browser: 'unknow', browserVersion: '0' };

function CheckBrowser() {
    var ua = navigator.userAgent.toLowerCase(),
        browserRegExp = {
            ie: /msie[ ]([\w.]+)/,
            firefox: /firefox[ |\/]([\w.]+)/,
            chrome: /chrome[ |\/]([\w.]+)/,
            safari: /version[ |\/]([\w.]+)[ ]safari/,
            opera: /opera[ |\/]([\w.]+)/
        };
    ZP.browser = 'unknow';
    ZP.browserVersion = '0';
    for (var i in browserRegExp) {
        var match = browserRegExp[i].exec(ua);
        if (match) {
            ZP.browser = i;
            ZP.browserVersion = match[1];
            break;
        }
    }

    var isIE11 = (ua.toLowerCase().indexOf("trident") > -1 && ua.indexOf("rv") > -1);
    if (isIE11) {
        var reIE11 = /[(rv):](\d+\.\d+)/;
        ////console.log(ua);
        var mt = reIE11.exec(ua);
        // //console.log(mt);
        var ie11ver = mt[1];
        // //console.log(ie11ver);
        ZP.browser = "ie";
        ZP.browserVersion = ie11ver;
    }
}


//function loaddiv(id, url) {
//    var target = $('#' + id);
//    // target.empty();
//    target.addClass("loader");
//    target.empty().append("加载中... ...");

//    $.ajax(
//    {
//        type: "get",
//        url: url,
//        cache: false,
//        error: function (xhr, textStatus, errorThrown) {
//            target.empty().append(textStatus);
//            target.removeClass("loader");
//        },
//        success: function (msg) {
//            target.empty().append(msg);
//            target.removeClass("loader");
//        }
//    });
//}







function slog(s) {
    try {
        console.log(s);
    }
    catch (e) { }
}

var cloneobj = (function () {
    // classify object
    var classof = function (o) {
        if (o === null) { return "null"; }
        if (o === undefined) { return "undefined"; }
        // I suppose Object.prototype.toString use obj.constructor.name
        // to generate string
        var className = Object.prototype.toString.call(o).slice(8, -1);
        return className;
    };

    var references = null;

    var handlers = {
        // Handle regexp and date even in shallow.
        'RegExp': function (reg) {
            var flags = '';
            flags += reg.global ? 'g' : '';
            flags += reg.multiline ? 'm' : '';
            flags += reg.ignoreCase ? 'i' : '';
            return new RegExp(reg.source, flags);
        },
        'Date': function (date) {
            return new Date(+date);
        },
        'Array': function (arr, shallow) {
            var newArr = [], i;
            for (i = 0; i < arr.length; i++) {
                if (shallow) {
                    newArr[i] = arr[i];
                } else {
                    // handle circular reference
                    if (references.indexOf(arr[i]) !== -1) {
                        continue;
                    }
                    var handler = handlers[classof(arr[i])];
                    if (handler) {
                        references.push(arr[i]);
                        newArr[i] = handler(arr[i], false);
                    } else {
                        newArr[i] = arr[i];
                    }
                }
            }
            return newArr;
        },
        'Object': function (obj, shallow) {
            var newObj = {}, prop, handler;
            for (prop in obj) {
                if (obj.hasOwnProperty(prop)) {
                    // escape prototype properties
                    if (shallow) {
                        newObj[prop] = obj[prop];
                    } else {
                        // handle circular reference
                        if (references.indexOf(obj[prop]) !== -1) {
                            continue;
                        }
                        // recursive
                        handler = handlers[classof(obj[prop])];
                        if (handler) {
                            references.push(obj[prop]);
                            newObj[prop] = handler(obj[prop], false);
                        } else {
                            newObj[prop] = obj[prop];
                        }
                    }
                }
            }
            return newObj;
        }
    };

    return function (obj, shallow) {
        // reset references
        references = [];
        // default to shallow clone
        shallow = shallow === undefined ? true : false;
        var handler = handlers[classof(obj)];
        return handler ? handler(obj, shallow) : obj;
    };
}());


///bootstrap 3.x modal center
/* center modal */
function centerModals(modal) {
    var $clone = modal.clone().css('display', 'block').appendTo('body');
    var top = Math.round(($clone.height() - $clone.find('.modal-dialog').height()) / 2);
    top = top > 0 ? top : 0;
    $('.modal').each(function (i) {
        $(this).find('.modal-dialog').css("margin-top", top * 0.6);//.css("margin-left", left);
    });
    $clone.remove();
}
function centerModalsAlign(modal) {
    var $clone = modal.clone().css('display', 'block').appendTo('body');
    var top = Math.round(($clone.width() - $clone.find('.modal-dialog').width()) / 2);
    top = top > 0 ? top : 0;
    $('.modal').each(function (i) {
        $(this).find('.modal-dialog').css("margin-left", top * 0.6);//.css("margin-left", left);
    });
    $clone.remove();
}

//function showMask() {
//    //var s = "<div class='mask-content'><div class='wBall' id='wBall_1'><div class='wInnerBall'></div></div><div class='wBall' id='wBall_2'><div class='wInnerBall'></div></div><div class='wBall' id='wBall_3'><div class='wInnerBall'></div></div><div class='wBall' id='wBall_4'><div class='wInnerBall'></div></div><div class='wBall' id='wBall_5'><div class='wInnerBall'></div></div></div><div class='mask-bg'></div>";

//    $("#divMaskLayer").modal('show');
//}

//function hideMask() {
//    $("#divMaskLayer").modal('hide');
//}
function fixedIe8Palceholder() {
    if (!('placeholder' in document.createElement('input'))) {

        $('input[placeholder],textarea[placeholder]').each(function () {
            var that = $(this),
                text = that.attr('placeholder');
            if (that.val() === "") {
                that.val(text).addClass('placeholder');
            }
            that.focus(function () {
                if (that.val() === text) {
                    that.val("").removeClass('placeholder');
                }
            })
                .blur(function () {
                    if (that.val() === "") {
                        that.val(text).addClass('placeholder');
                    }
                })
                .closest('form').submit(function () {
                    if (that.val() === text) {
                        that.val('');
                    }
                });
        });
    }
}

function arrRemove(arr, value) {
    var index = $.inArray(value, arr);

    if (index >= 0) {
        //arrayObject.splice(index,howmany,item1,.....,itemX)

        //参数    描述
        //index  必需。整数，规定添加/删除项目的位置，使用负数可从数组结尾处规定位置。
        //howmany 必需。要删除的项目数量。如果设置为 0，则不会删除项目。
        //item1, ..., itemX 可选。向数组添加的新项目。
        arr.splice(index, 1);
    }
}


function getTitleRect() {

    //var title = $('window-drag').element;

    //$('window-drag').

    //var height = title.height();

//    var winLeft=

}