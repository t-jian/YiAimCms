<template>
  <div
    class="createPost-container appmsg_input_area"
    v-show="isShow"
    :style="isShow ? '' : 'display: none;'"
  >
    <el-form
      ref="postForm"
      :model="postForm"
      :rules="rules"
      class="form-container"
    >
      <div id="js_title_main" class="js_title_main input_box" prop="title">
        <input
          id="title"
          type="text"
          v-model="postForm.title"
          placeholder="请在这里输入标题"
          class="frm_input title"
          name="title"
          max-length="64"
          @focus="titleFocus"
          @blur="blur"
        />
        <em
          :class="calcTitleLength > 64 ? 'frm_counter warn' : 'frm_counter'"
          style="right: -40px"
          v-show="isTitleWordCount"
          >{{ calcTitleLength }}/64</em
        >
      </div>
      <div id="js_author_area" class="js_author_container input_box">
        <input
          id="author"
          type="text"
          v-model="postForm.author"
          placeholder="请输入作者"
          class="frm_input js_author"
          name="author"
          max-length="8"
          autocomplete="off"
          @focus="authorFocus"
          @blur="blur"
        />
        <em
          :class="calcAuthorLength > 8 ? 'frm_counter warn' : 'frm_counter'"
          style="right: -40px"
          v-show="isAuthorWordCount"
          >{{ calcAuthorLength }}/8</em
        >
      </div>
      <ueditor
        v-if="firstLoading"
        v-model="postForm.content"
        :config="editorConfig"
        :editor-dependencies="editorDependencies"
        id="ueditor"
        :destroy="true"
        @ready="readyUeditor"
      ></ueditor>
      <div class="article_setting_area">
        <div id="js_cover_description_area">
          <div class="setting-group__title">封面和摘要</div>
          <div class="setting-group__content">
            <div
              id="js_cover_area"
              class="setting-group__cover setting-group__cover_primary"
            >
              <el-upload
                class="select-cover__btn"
                action="/api/app/file/upload"
                list-type="picture-card"
                :show-file-list="false"
                :on-success="handleImageSuccess"
              >
                <img
                  v-if="postForm.thumImg"
                  :src="postForm.thumImg"
                  class="thum_img"
                />
                <i v-else class="el-icon-plus avatar-uploader-icon"></i>
              </el-upload>
            </div>
            <div id="js_description_area" class="setting-group__abstract">
              <span
                class="frm_textarea_box js_description_span"
                id="js_description_span"
              >
                <textarea
                  id="js_description"
                  v-model="postForm.digest"
                  placeholder="选填，摘要会在订阅号消息、转发链接等文章外的场景显露，帮助读者快速了解内容，如不填写则默认抓取正文前54字"
                  class="frm_textarea"
                  name="digest"
                  max-length="120"
                ></textarea>
                <em
                  id="desc_frm_counter"
                  :class="
                    calcDescLength > 120 ? 'frm_counter warn' : 'frm_counter'
                  "
                  >{{ calcDescLength }}/120</em
                >
              </span>
            </div>
          </div>
        </div>
      </div>
      <div class="mrow">文章设置 <span style="font-size:12px;color: #999;">(分类、发布时间)</span>  </div>
      <div class="mrow" style="display: flex; justify-content: space-between;">
        <el-select v-model="postForm.columnId" placeholder="选择分类">
          <el-option
            v-for="item in options"
            :key="item.id"
            :label="item.title"
            :value="item.id"
          >
          </el-option>
        </el-select>
        <el-date-picker v-model="postForm.publishDate" type="datetime"  placeholder="发布时间"></el-date-picker>
      </div>
      <div class="article_tag_area">
        <el-tag
          :key="tag"
          v-for="tag in dynamicTags"
          closable
          :disable-transitions="false"
          @close="handleClose(tag)"
        >
          {{ tag }}
        </el-tag>
        <el-input
          class="input-new-tag"
          v-if="inputVisible"
          v-model="inputValue"
          ref="saveTagInput"
          size="small"
          @keyup.enter="handleInputConfirm"
          @blur="handleInputConfirm"
        >
        </el-input>
        <el-button v-else class="button-new-tag" size="small" @click="showInput"
          >添加标签</el-button
        >
      </div>
      
      <div style="margin-bottom: 120px"></div>
      <div id="bottom_main" class="tool_area_wrp">
        <div class="js_bot_bar tool_area">
          <el-button v-loading="loading" type="success" @click="submitForm"
            >提交</el-button
          >
        </div>
      </div>
    </el-form>
  </div>
</template>

<script>
import Sticky from "@/components/Sticky";
import article from "@/api/article";
import { GetAllColum } from "@/api/column";
import ueditor from "vue-ueditor-wrap";


const defaultForm = {
  id: 0,
  columnId: 1,
  title: "",
  author: "t-jian",
  source: "",
  linkUrl: "",
  sort: 0,
  thumImg: "",
  status: 0,
  digest: "",
  content: '',
  tags: "",
  publishDate:new Date()
};

export default {
  name: "ArticleDetail",
  components: {
    Sticky,
    ueditor,
  },
  props: {
    isEdit: {
      type: Boolean,
      default: false,
    },
  },

  data() {
    const validateRequire = (rule, value, callback) => {
      if (value === "") {
        callback(new Error(rule.field + "为必传项"));
      } else {
        callback();
      }
    };
    return {
      postForm: Object.assign({}, defaultForm),
      loading: false,
      firstLoading:false,
      rules: { title: [{ validator: validateRequire ,trigger: 'blur'}] },
      tempRoute: {},
      options: [{ id: 0, parentId: 0, sort: 0, title: "请选择分类" }],
      isTitleWordCount: false,
      isAuthorWordCount: false,
      dynamicTags: [],
      inputVisible: false,
      inputValue: "",
      editorInstance: null,
      isShow: false,
      editorConfig:"",
      editorDependencies:"",
    };
  },
  computed: {
    calcDescLength() {
      return this.postForm.digest.length;
    },
    calcTitleLength() {
      return this.postForm.title.length;
    },
    calcAuthorLength() {
      return this.postForm.author.length;
    },
  },
  created() {
    let _this = this;
    this.GetClassList();
     if (this.isEdit) {
      let id = this.$route.params && this.$route.params.id;
      article
        .get(id)
        .then((res) => {
          this.postForm = res;
          this.postForm.publishDate=new Date(Number(this.postForm.publishDate)*1000);
          this.postForm.id = id;
          if (res.tags != "") this.dynamicTags = res.tags.split(",");
          _this.setPageTitle();
          _this.initEditor();
          
        })
        .catch(() => {
          this.$message.error("文章信息获取错误");
          this.$router.push({ path: "/cms/article" });
        });
    }else{
      this.initEditor();
    }
    this.tempRoute = Object.assign({}, this.$route);
  },
  beforeCreate() {
    document.getElementsByTagName("body")[0].className = "mapp";
  },
  beforeDestroy() {
    document.getElementsByTagName("body")[0].className = "";
    this.isShow = false;
    let eduiBox = document.getElementById("edui_fixedlayer");
    if (eduiBox != undefined) eduiBox.remove();
    if (this.editorInstance) this.editorInstance.setHide();
    let ueditorBox = document.getElementById("ueditor");
    if (ueditorBox != undefined) ueditorBox.remove();
  },
  methods: {
    initEditor(){
      this.editorConfig = {
      toolbars: [
        [
          "source",
          "|",
          "undo",
          "redo",
          "|",
          "bold",
          "italic",
          "underline",
          "fontborder",
          "strikethrough",
          "superscript",
          "subscript",
          "removeformat",
          "formatmatch",
          "autotypeset",
          "blockquote",
          "pasteplain",
          "|",
          "forecolor",
          "backcolor",
          "insertorderedlist",
          "insertunorderedlist",
          "selectall",
          "cleardoc",
          "|",
          "rowspacingtop",
          "rowspacingbottom",
          "lineheight",
          "|",
          "customstyle",
          "paragraph",
          "fontfamily",
          "fontsize",
          "|",
          "directionalityltr",
          "directionalityrtl",
          "indent",
          "|",
          "justifyleft",
          "justifycenter",
          "justifyright",
          "justifyjustify",
          "|",
          "touppercase",
          "tolowercase",
          "|",
          "link",
          "unlink",
          "anchor",
          "|",
          "imagenone",
          "imageleft",
          "imageright",
          "imagecenter",
          "|",
          "simpleupload",
          "insertimage",
          "emotion",
          "scrawl",
          "insertvideo",
          "music",
          "attachment",
          "map",
          "insertcode",
          "pagebreak",
          "background",
          "|",
          "horizontal",
          "spechars",
          "|",
          "inserttable",
        ],
      ],
     //  UEDITOR_HOME_URL:"https://cdn.jsdelivr.net/gh/t-jian/static/theme/ueditor-1.4.3/",
      UEDITOR_HOME_URL: "/UEditor/",
      serverUrl: "/api/UEditor/Upload",
      initialFrameWidth: null,
      initialFrameHeight: 589,
      autoHeightEnabled: true,
    };
    this.editorDependencies = [
        'ueditor.config.js',
        'ueditor.all.js',
        'ueditor.parse.js',
    ];
    this.firstLoading=true;
    },
    readyUeditor(editorInstance) {
      let _this = this;
      this.editorInstance = editorInstance;
     
      let m = document.querySelector(".edui-editor-iframeholder");
      if (m != null) {
        let mdiv = document.createElement("div");
        mdiv.className = "editor_content_placeholder";
        mdiv.id = "editor_content_placeholder_tip";
        mdiv.innerText = "从这里开始正文";
        m.prepend(mdiv);
        this.isShow = true;
      }
      _this.showOrHideUeditorMainTip(_this.postForm.content.length > 0);

      let eduiBox = document.getElementById("edui_fixedlayer");
      if (eduiBox != undefined) {
        eduiBox.remove();
      }

      this.editorInstance.addListener("focus", function (editor) {
        this.isShow = false;
        _this.showOrHideUeditorMainTip();
      });
      this.editorInstance.addListener("blur", function (editor) {
        _this.showOrHideUeditorMainTip(_this.postForm.content.length > 0);
      });
    },
    titleFocus() {
      this.isTitleWordCount = true;
    },
    authorFocus() {
      this.isAuthorWordCount = true;
    },
    showOrHideUeditorMainTip(b = true) {
      let tip = document.querySelector("#editor_content_placeholder_tip");
      if (b){ tip.style.display = "none";}
      else {tip.style.display = "";}
    },
    blur() {
      if (this.postForm.title.length <= 64) {
        this.isTitleWordCount = false;
      }
      if (this.postForm.author.length <= 8) {
        this.isAuthorWordCount = false;
      }
    },
    handleImageSuccess(res) {
      if (res.success) {
        this.postForm.thumImg = res.result.fileUrl;
        this.$message.success("图片上传成功");
      } else {
        this.$message.error("图片上传失败");
      }
    },
    GetClassList() {
      GetAllColum().then((res) => {
        this.options.push(...res);
      });
    },

    setPageTitle() {
      const title = "编辑文章";
      document.title = `${title} - ${this.postForm.id}`;
    },
    submitForm() {
      var _this = this;
      this.$refs.postForm.validate((valid) => {
        if (valid) {
          _this.loading = true;
          _this.postForm.tags = _this.dynamicTags.join(",");
        
          if (this.isEdit) {
            article
              .updata(this.postForm.id, this.postForm)
              .then((res) => {
                this.loading = false;
                if (res.success) {
                  this.$message.success("更新文章成功");
                  _this.$router.push({ path: "/cms/article" });
                } else {
                  this.$message.error(res.message);
                }
              })
              .catch(() => {
                this.loading = false;
              });
          } else {
            article
              .add(this.postForm)
              .then((res) => {
                this.loading = false;
                if (res.success) {
                  this.$message.success("发布文章成功");
                  _this.$router.push({ path: "/cms/article" });
                } else {
                  this.$message.error(res.message);
                }
              })
              .catch(() => {
                this.loading = false;
              });
          }
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    draftForm() {
      if (this.postForm.title.length === 0) {
        this.$message({
          message: "请填写必要的标题",
          type: "warning",
        });
        return;
      }
      this.$message({
        message: "保存成功",
        type: "success",
        showClose: true,
        duration: 1000,
      });
      this.postForm.status = "draft";
    },

    handleClose(tag) {
      this.dynamicTags.splice(this.dynamicTags.indexOf(tag), 1);
    },

    showInput() {
      this.inputVisible = true;
      this.$nextTick((_) => {
        this.$refs.saveTagInput.$refs.input.focus();
      });
    },

    handleInputConfirm() {
      let inputValue = this.inputValue;
      if (inputValue) {
        this.dynamicTags.push(inputValue);
      }
      this.inputVisible = false;
      this.inputValue = "";
    },
  },
};
</script>
<style>
@import "~@/styles/articleEdit.css";
</style>
