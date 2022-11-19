<template>
    <div class="ant-upload-preview">
        <div style="width: 100%">
            <el-upload ref="upload" :show-file-list="false" action :before-upload="beforeUpload" :http-request="handleChange">
                 <el-button id="initSlide" type="primary"  style="display: none;">点击上传</el-button>
            </el-upload>
        </div>
        <cropper-modal ref="CropperModal" :fileName="file.name" :contentType="file.type"  @cropper-no="handleCropperClose" @cropper-ok="handleCropperSuccess"></cropper-modal>
    </div>
</template>
<script>
import CropperModal from './CropperModal'
export default {
  name: 'ImageCropper',
  components: {
    CropperModal
  },
  props: {
    //图片裁切配置
    options: {
      type: Object,
      default: function() {
        return {
          autoCrop: true, 
          autoCropWidth: 180,
          autoCropHeight: 180, 
          fixedBox: false, 
          centerBox:true,
          previewsCircle: true, 
          title: '修改头像'
        }
      }
    },
    imgSize: {
      type: Number,
      default: 2
    },
    imageUrl: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      file:{name:"",type:""},
      loading: false,
      isStopRun: false
    }
  },

  methods: {
   clickShowUpload(){
     var e = document.createEvent("MouseEvents");
      e.initEvent("click", true, true);
      document.getElementById("initSlide").dispatchEvent(e);
   },
    //从本地选择文件
    handleChange(info) {
      if (this.isStopRun) {
        return
      }
      this.loading = true
      const { options } = this
      this.file=info.file
      this.getBase64(info.file, imageUrl => {
        const target = Object.assign({}, options, {
          img: imageUrl
        })
        this.$refs.CropperModal.edit(target)
      })
    },
    // 上传之前 格式与大小校验
    beforeUpload(file) {
      this.isStopRun = false
      var fileType = file.type
      if (fileType.indexOf('image') < 0) {
        this.$message.warning('请上传图片')
        this.isStopRun = true
        return false
      }
      const isJpgOrPng =
        file.type === 'image/jpeg' ||
        file.type === 'image/png' ||
        file.type === 'image/jpg'
      if (!isJpgOrPng) {
        this.$message.error('你上传图片格式不正确!')
        this.isStopRun = true
      }
      const isLtSize = file.size < this.imgSize * 1024 * 1024
      if (!isLtSize) {
        this.$message.error('图片大小不能超过' + this.imgSize + 'MB!')
        this.isStopRun = true
      }
      return isJpgOrPng && isLtSize
    },
    //获取服务器返回的地址
    handleCropperSuccess(data,callback) {
      //将返回的数据回显
      this.loading = false
      this.$emit('crop-upload-success', data,callback)
    },
    // 取消上传
    handleCropperClose() {
      this.loading = false
      this.$emit('crop-upload-close')
    },
    getBase64(img, callback) {
      const reader = new FileReader()
      reader.addEventListener('load', () => callback(reader.result))
      reader.readAsDataURL(img)
    }
  }
}
</script>



