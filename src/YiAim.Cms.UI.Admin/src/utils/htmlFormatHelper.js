/***标签格式助手 */

class htmlFormatHelper {
    /**
     * 清楚所有标签
     * @param {*} content 
     * @returns 
     */
    clearAllLable(content) {
            let regex = /<[^>]+>/g
            return content.replace(regex, '')
        }
        /**
         * 清除标签包含内容
         * @param {*} content  内容
         * @param {*} lable  html 常规标签
         * @param {*} replaceTxt  替换内容
         * @returns content
         */
    clearLable(content, lable, replaceTxt = "") {
            let regex = new RegExp(`<${lable}[^>]*?>[\\s\\S]*?<\/${lable}>`, "gims")
            return content.replace(regex, replaceTxt)
        }
        /**
         * 清除a标签 只保留内容
         * @param {*} content 
         * @returns 
         */
    clearALable(content) {
            let regex = /<a.*?>([\s\S]*?)<\/a>/gims
            return content.replace(regex, "$1")
        }
        /**
         * 清除所有标签里面的属性
         * @param {*} content 
         * @param {*} isContainImg  默认不包含img 标签
         * @returns 
         */
    clearAllLableAttribute(content, isContainImg = true) {
            let regex = isContainImg ? /(<(?!img)[a-zA-Z0-9]+).*?(>|\/>)/gims : /(<[a-zA-Z0-9]+).*?(>|\/>)/gims
            return content.replace(regex, "$1$2")
        }
        /**
         * 清除img标签里面的除src外的属性
         * @param {*} content 
         * @returns 
         */
    clearImgLableAttribute(content) {
        let regex = /(^<img).*?(src[^=]*=[\"']*[^\"'>]+[\"']*).*?(>|\/>)/gims
        return content.replace(regex, "$1 $2  alt='' $3")
    }

    /**
     * 清除html注释 
     */
    clearNotes(content) {
            let regex = /(\/{2,}.*?(\r|\n))|(\/\*(\n|.)*?\*\/)/gims
            let reg2 = /(<!--.*?-->)/gims
            return content.replace(reg2, '').replace(regex, '')
        }
        /**
         * 清除多余的空格 大于一个空格
         * @param {} content 
         * @returns 
         */
    clearExtraSpace(content) {
            let regex = /\s(?=\s)/gims
            return content.replace(regex, '').trim()
        }
        /**
         * 清除多余的换行 大于一个换行
         * @param {} content 
         * @returns 
         */
    clearExtraLine(content) {
            let regex = /\n(?=\n)/gims
            return content.replace(regex, '').trim()
        }
        /**
         * 清除多余的nbsp 大于一个nbsp
         * @param {} content 
         * @returns 
         */
    clearExtranbsp(content) {
        let regex = /\&nbsp;(?=\&nbsp;)/gims
        return content.replace(regex, '').trim()
    }

    /**
     * 替换标签
     * @param {*} content  内容
     * @param {*} lable  html 常规标签
     * @param {*} replaceTxt  替换内容
     * @returns 
     */
    replaceLable(content, lable, replaceTxt = "") {
        let regex = new RegExp(`(<)${lable}.*?(>)|(</).?${lable}(>)`, "gims")
        replaceTxt = replaceTxt === "" ? replaceTxt : `$1$3${replaceTxt}$2$4`
        return content.replace(regex, replaceTxt)
    }

    /**
     * 替换标签内的属性
     * @param {*} content 
     * @param {*} lable 
     * @param {*} replaceTxt 
     * @returns 
     */
    replaceLableAttr(content, lable, replaceTxt = "") {
        let regex = new RegExp(`${lable}=*=[\"']*[^\"'>]+[\"']*`, "gims")
        return content.replace(regex, replaceTxt)
    }

    /**
     * 正则表达式替换
     * @param {*} content 
     * @param {*} regex 
     * @param {*} replaceTxt 
     * @returns 
     */
    replaceRegex(content, regex, replaceTxt = "") {
            regex = eval(regex)
            return content.replace(regex, replaceTxt)
        }
        /**
         * 判断是否为正则表达式
         * @param {} v 
         * @returns 
         */
    isRegExp(v) {
        return Object.prototype.toString.call(v) === '[object RegExp]';
    }

}

export default new htmlFormatHelper()