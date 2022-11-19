<template>
  <el-dialog
    :visible.sync="visible"
    :title="options.title"
    :close-on-click-modal="false"
    width="890px"
    @close="cancelHandel"
  >
    <div class="cropper_container">
      <div class="cropper_box">
        <vue-cropper
          ref="cropper"
          :img="options.img"
          :info="true"
          :autoCrop="options.autoCrop"
          :autoCropWidth="options.autoCropWidth"
          :autoCropHeight="options.autoCropHeight"
          :fixedNumber="options.fixedNumber"
          :fixed="options.fixed"
          :full="options.full"
          :fixedBox="options.fixedBox"
          :canMove="options.canMove"
          :canMoveBox="options.canMoveBox"
          :original="options.original"
          :centerBox="options.centerBox"
          :high="options.high"
          :canScale="options.canScale"
          :infoTrue="options.infoTrue"
          :maxImgSize="options.maxImgSize"
          :enlarge="options.enlarge"
          :mode="options.mode"
          
          @realTime="realTime"
          @cropMoving="cropMoving"
        >
        </vue-cropper>
      </div>
      <div class="preview_box">
        <div
          :class="
            options.previewsCircle
              ? 'avatar-upload-preview'
              : 'avatar-upload-preview_range'
          "
        >
            <img :src="previews.url" :style="previews.img"/>
        </div>
      </div>
    </div>
    <template slot="footer">
      <el-button size="mini" @click="cancelHandel">取消</el-button>
      <el-button
        size="mini"
        type="primary"
        :loading="confirmLoading"
        @click="okHandel"
        >保存</el-button
      >
    </template>
  </el-dialog>
</template>
<script>
import { VueCropper } from "vue-cropper"
import {UpImage} from "@/api/file"
export default {
  name: "CropperModal",
  components: {
    VueCropper,
  },
  props: {
    fileName:{
      type:String,
      default:""
    },
    contentType:{
      type:String,
      default:""
    }
  },
  data() {
    return {
      visible: false,
      img: null,
      confirmLoading: false,
      axis:{},
      options: {
        img: "", //裁剪图片的地址
        autoCrop: true, //是否默认生成截图框
        autoCropWidth: 180, //默认生成截图框宽度
        autoCropHeight: 180, //默认生成截图框高度
        previewsCircle: true, //预览图是否是原圆形
        title: "修改头像",
        canScale: false,
        fixedNumber: [1, 1],
        fixed: true, //是否开启截图框宽高固定比例
        full: false, //false按原比例裁切图片
        fixedBox: false, //固定截图框大小，不允许改变
        canMove: false, //上传图片是否可以移动
        canMoveBox: true, //截图框能否拖动
        original: false, //上传图片按照原始比例渲染
        centerBox: false, //截图框是否被限制在图片里面
        height: true, //是否按照设备的dpr 输出等比例图片
        infoTrue: false, //true为展示真实输出图片宽高，false展示看到的截图框宽高
        maxImgSize: 3000, //限制图片最大宽度和高度
        enlarge: 1, //图片根据截图框输出比例倍数
        mode: "contain", //图片默认渲染方式
        isShowPreview: true, //是否显示预览区域
      },
      previews: {},
      preview_img_style:{}
    };
  },

  methods: {
    edit(record) {
      const { options } = this;
      this.visible = true;
      this.options = Object.assign({}, options, record);
    },
    /**
     * 取消截图
     */
    cancelHandel() {
      this.confirmLoading = false;
      this.visible = false;
      this.$emit("cropper-no");
    },
    /**
     * 确认截图
     */
    okHandel() {
      const that = this;
      that.confirmLoading = true;
      // 获取截图的base64 数据
      this.$refs.cropper.getCropData((data) => {
           let formData = new FormData();
           formData.append('filename',this.fileName)
           formData.append('name',this.fileName)
           formData.append('ContentType',this.contentType)
           //将剪裁后base64的图片转化为file格式
           let fileRes = this.convertBase64UrlToBlob(data)
           formData.append("file", fileRes)
            UpImage(formData).then(res=>{
             that.$emit("cropper-ok", res,(visible)=>{
               this.visible=visible
             });
            }).catch(error=>{
              console.log(error,9999)
              that.$message.error(err);
            })
      });
    },
    //移动框的事件
    realTime(data) {
      this.previews = data
      // let h=data.img.height
      // let w=data.img.width
      // let fixedBoxH=data.h
      // if(w.indexOf("px")>-1){
      //   w=Number(w.replace("px",""))
      // }
      //  if(h.indexOf("px")>-1){
      //   h=Number(h.replace("px",""))
      // }
      
      // let yScale=Math.round(this.axis.y1+fixedBoxH/2)
      // console.log(data,this.axis.y1,yScale,555)
      // let tY=Math.round((yScale/h)*10000)/100
      // this.preview_img_style="translate(0%, -"+tY+"%)"
      // console.log(this.preview_img_style,6666)
    },
    cropMoving(data){
      this.axis=data.axis
    },
    convertBase64UrlToBlob(urlData) {
            let bytes = window.atob(urlData.split(',')[1]);//去掉url的头，并转换为byte
            //处理异常,将ascii码小于0的转换为大于0
            let ab = new ArrayBuffer(bytes.length);
            let ia = new Uint8Array(ab);
            for (var i = 0; i < bytes.length; i++) {
                ia[i] = bytes.charCodeAt(i);
            }
            return new Blob([ab], { type: 'image/jpeg'});
        }
  },
};
</script>

<style lang="scss" scoped>
.cropper_container {
  position: relative;
  display: flex;
  .cropper_box {
    width: 554px;
    height: 416px;
  }
  .preview_box {
    flex: 1;
    display: flex;
    align-items: center;
  }
}

.avatar-upload-preview_range,
.avatar-upload-preview {
  width: 259px;
  height: 110px;
  text-align: center;
  margin: 0 auto;
  overflow: hidden;
  img {
    width: 100%;
    height: auto;
    object-fit: cover;
    object-position: 50% 50%;
  }
}
.avatar-upload-preview_range {
  border-radius: 0;
}
</style>


