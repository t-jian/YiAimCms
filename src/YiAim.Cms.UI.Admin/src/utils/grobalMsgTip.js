import { Message, MessageBox, Loading } from 'element-ui'

const MessageTip = {
    success: function name(msg) {
        Message.success({
            message: msg,
            duration: 2000
        });
    },
    error(msg) {
        Message.error({
            message: msg,
            duration: 2000
        });
    },
    info(msg) {
        Message.info({
            message: msg,
            duration: 2000
        });
    },
    warning(msg) {
        Message.warning({
            message: msg,
            duration: 2000
        });
    },
    delete(succesFn, cancelFn) {
        MessageBox.confirm('此操作将永久删除, 是否继续?', '系统提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning',
        }).then(() => {
            if (typeof succesFn == "function") {
                succesFn();
            }
        }).catch(() => {
            if (typeof cancelFn == "function") {
                cancelFn();
            }
        });
    },
    loading() {
        return Loading.service({
            lock: true,
            text: "Loading",
            spinner: "el-icon-loading",
            background: "rgba(0, 0, 0, 0.56)",
        })
    }
}
export default {
    MessageTip
}