import { getBlogById, createBlog, updateBlog } from "@/api/blogs/blog"
import { getAllCategory } from "@/api/blogs/category"
import{upload}from "@/api/file"
import ueditor from "vue-ueditor-wrap"
import htmlFormat from "../../utils/htmlFormatHelper"
export default {
  components: {
    ueditor,
  },
  props: {
    isEdit: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isShowEdit: false,
      isEditorInit: false,
      titleMaxLen: 120,
      descMaxLen: 160,
      isTitleWordCount: false,
      isAuthorWordCount: false,
      categories: [],
      confirmLoading: false,
      publishDate: new Date(),
      article: {
        id: 0,
        categoryId: 0,
        title: "",
        author: "",
        source: "",
        linkUrl: "",
        taxis: 0,
        thumbImg: "",
        status: 0,
        digest: "",
        content: "",
        tags: "",
        publishDate: "",
      },
      editorConfig: "",
      editorDependencies: [
        "ueditor.config.js",
        "ueditor.all.js",
        "ueditor.parse.js",
      ],
      dynamicTags: [],
      inputVisible: false,
      inputValue: "",
      uploadLoading: false,
      isShowContentTip: true,
      thumbList: [],
      submitTxts: ["发布", "提交修改"],
      articleImgvisible: false,
      currentContentImgs: [],
      currentContentImgIndex: -1,
      formatVisible: false,
      checkedList: [],
      plainOptions: [
        "所有标签",
        "注释",
        "多余空格",
        "所有标签属性（不含img）",
        "img（保留src）",
        "a(保留内容)",
        "换行",
        "video",
        "form",
        "iframe",
        "style",
        "script",
        "标签class",
        "标签style",
      ],
      formatRichLableInput: "",
      //替换的标签或正则表达式
      replaceLableRegexInput: "",
      //替换的内容
      replaceRichInput: "",
    };
  },
  created() {
    this.showTemplList = [];
    this.confirmLoading = true;
    this.initEditor();

    let id = 0;
    if (this.isEdit) {
      id = this.$route.params && this.$route.params.id;
    }
    Promise.all([getAllCategory(), this.getArticle(id)])
      .then((res) => {
        this.categories = res[0];
        if (this.categories.length > 0) {
          this.article.categoryId = this.categories[0].id;
        }
        console.log(res[1],666)
        if (res[1] != null) {
          let detail = res[1];
          this.article = detail;
          this.article.content = decodeURIComponent(this.article.content);
          if (detail.thumbImg != "") {
            var index = 0;
            var vv = [];
            for (var img of detail.thumbImg.split("|")) {
              index++;
              vv.push({
                uid: index + "",
                name: img,
                status: "done",
                url: img,
                thumbUrl: img,
              });
            }
            this.thumbList = vv;
          }
          // if (detail.articleTags != null) {
          //   for (var item of detail.articleTags) {
          //     this.dynamicTags.push(item.tag.name);
          //   }
          // }
        }
        this.confirmLoading = false;
      })
      .finally(() => {
        this.confirmLoading = false;
      });
  },
  computed: {
    calcDescLength() {
      return this.article.digest == undefined ? 0 : this.article.digest.length;
    },
    calcTitleLength() {
      return this.article.title == undefined ? 0 : this.article.title.length;
    },
    calcAuthorLength() {
      return this.article.author == undefined ? 0 : this.article.author.length;
    },
  },
  mounted() {
    //监听滚动条
    window.addEventListener("scroll", this.scrolling);
  },

  beforeDestroy() {
    //this.editorInstance.destroy()
    window.removeEventListener('scroll', this.scrolling, false);
  },
  watch: {
    "article.content"(newValue, oldValue) {
      this.isShowContentTip =
        newValue == undefined ? false : newValue.length === 0;
    },
  },
  methods: {
    scrolling() {
      //动态设置编辑器工具栏的距离顶部的位置，主要是使用js控制css变量（--topBar）
      let scrollTop =
        window.pageYOffset ||
        document.documentElement.scrollTop ||
        document.body.scrollTop;
      let t = 82 - scrollTop;
      document.documentElement.style.setProperty(
        "--topBar",
        (t > 0 ? t : 0) + "px"
      );
    },
    titleFocus() {
      this.isTitleWordCount = true;
    },
    blur() {
      if (this.article.title.length <= this.titleMaxLen) {
        this.isTitleWordCount = false;
      }
      if (this.article.author.length <= 8) {
        this.isAuthorWordCount = false;
      }
    },
    getArticle(id) {
      if (this.isEdit) {
        return getBlogById(id);
      } else {
        return new Promise((resolve, reject) => {
          resolve(null);
        });
      }
    },
    removeFile(file) {
      this.thumbList = this.thumbList.filter((n) => n.uid != file.uid);
    },
    /**文件上传相关 start */
    beforeUpload(file) {
      const isJpgOrPng =
        file.type === "image/jpeg" || file.type === "image/png";
      if (!isJpgOrPng) {
        this.$message.error("图片格式不正确");
      }
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isLt2M) {
        this.$message.error("图片大小不能超过2MB!");
      }
      return isJpgOrPng && isLt2M;
    },
    upload(e) {
      var filePath = e.file.name;
      var index = filePath.lastIndexOf(".");
      var ext = filePath.substr(index);
      const formData = new FormData();
      formData.append("file", e.file);
      upload(formData).then((res) => {
            let img =res;
            this.thumbList.push({
              uid: e.file.uid,
              name: img,
              status: "done",
              url: img,
              thumbUrl: img,
            });
        })
        .catch((err) => {
          e.onError(err);
        });
    },
    /***end */
    initEditor() {
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
        UEDITOR_HOME_URL: "/UEditor/",
        serverUrl: "https://localhost:44377/api/UEditor/upload",
        initialFrameWidth: null,
        initialFrameHeight: 589,
        autoHeightEnabled: true,
        allowDivTransToP: false, //阻止其他标签转成p标签
      };
    },
    readyUeditor(editorInstance) {
      this.isEditorInit = true;
      console.info("编辑器初始化完成");
    },
    submitForm(status) {
      if (this.article.title == "") {
        this.$mtip.error("标题不能为空");
        return false;
      }
      this.confirmLoading = true;

      this.article.tags = this.dynamicTags.join(",");
      let imgs = this.thumbList.map((n) => {
        return n.url;
      });
      this.article.thumbImg = imgs.join("|");
      this.article.publishDate = parseInt(
        new Date(this.publishDate).getTime() / 1000
      );
      this.article.status = status;
      this.confirmLoading = true;
      if (this.isEdit) {
        updateBlog(this.article).then((res) => {
            this.confirmLoading = false;
            this.$mtip.success("修改成功");
            this.rollback();
        }).catch(()=>{
            this.confirmLoading = false;
        });
      } else {
        createBlog(this.article).then((res) => {
          this.confirmLoading = false;
          this.$mtip.success("添加成功");
          this.rollback();
        }).catch(()=>{
            this.confirmLoading = false;
        });
      }
    },
    rollback() {
      //返回
      this.$router.push({ path: "/cms/blog" });
    },
    /**tag */
    handleInputConfirm() {
      console.log(111);
      let inputValue = this.inputValue;
      if (inputValue) {
        this.dynamicTags.push(inputValue);
      }
      this.inputVisible = false;
      this.inputValue = "";
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
    /**tag end */
    getDetailImgs() {
      this.currentContentImgs = [];
      let content = this.article.content;
      var imgs = [];
      content.replace(
        /<img [^>]*src=['"]([^'"]+)[^>]*>/g,
        function (match, capture) {
          imgs.push(capture);
        }
      );
      if (imgs.length === 0) {
        message.info("正文中不存在图片");
        return;
      }
      this.currentContentImgs = imgs;
      this.articleImgvisible = true;
    },
    chooseCurrentContentImg(i) {
      this.currentContentImgIndex = i;
    },
    currentContentImgOk() {
      let img = this.currentContentImgs[this.currentContentImgIndex];
      this.thumbList.push({
        uid: img,
        name: img,
        status: "done",
        url: img,
        thumbUrl: img,
      });
      this.currentContentImgIndex = -1;
      this.articleImgvisible = false;
      this.currentContentImgs = [];
    },
    showDrawer() {
      this.formatVisible = true;
      this.checkedList = [];
      this.formatRichLableInput = "";
      this.replaceLableRegexInput = "";
      this.replaceRichInput = "";
    },
    onClose() {
      this.formatVisible = false;
    },

    formatRichText() {
      //清除格式
      if (this.checkedList.length == 0 && this.formatRichLableInput == "") {
        message.info("请输入或选择清除格式");
        return;
      }
      this.confirmLoading = true;
      let content = this.article.content;
      if (
        this.formatRichLableInput != "" &&
        htmlFormat.isRegExp(this.formatRichLableInput)
      ) {
        content = htmlFormat.replaceRegex(
          content,
          this.replaceLableRegexInput,
          ""
        );
      }
      if (this.checkedList.length > 0) {
        let checkedList = this.checkedList;
        if (checkedList.indexOf(this.plainOptions[0]) > -1) {
          content = this.chooseFormatType(this.plainOptions[0], content);
        } else {
          for (var lable of checkedList) {
            content = this.chooseFormatType(lable, content);
          }
        }
      }
      message.success("标签清除成功");
      this.article.content = content;
      this.confirmLoading = false;
      this.formatVisible = false;
    },
    chooseFormatType(lable, content) {
      let plainOptions = this.plainOptions;
      switch (lable) {
        case plainOptions[0]:
          content = htmlFormat.clearAllLable(content);
          break;
        case plainOptions[1]:
          content = htmlFormat.clearNotes(content);
          break;
        case plainOptions[2]:
          content = htmlFormat.clearExtranbsp(content);
          content = htmlFormat.clearExtraSpace(content);
          break;
        case plainOptions[3]:
          content = htmlFormat.clearAllLableAttribute(content);
          break;
        case plainOptions[4]:
          content = htmlFormat.clearImgLableAttribute(content);
          break;
        case plainOptions[5]:
          content = htmlFormat.clearALable(content);
          break;
        case plainOptions[6]:
          content = htmlFormat.clearExtraLine(content);
          break;
        case plainOptions[7]:
          content = htmlFormat.clearLable(content, "video");
          break;
        case plainOptions[8]:
          content = htmlFormat.clearLable(content, "form");
          break;
        case plainOptions[9]:
          content = htmlFormat.clearLable(content, "iframe");
          break;
        case plainOptions[10]:
          content = htmlFormat.clearLable(content, "style");
          break;
        case plainOptions[11]:
          content = htmlFormat.clearLable(content, "script");
          break;
        case plainOptions[12]:
          content = htmlFormat.replaceLableAttr(content, "class");
          break;
        case plainOptions[13]:
          content = htmlFormat.replaceLableAttr(content, "style");
          break;

        default:
          break;
      }
      return content;
    },

    replaceRichText() {
      let content = this.article.content;
      //替换文本内容
      if (this.replaceLableRegexInput == "") {
        message.info("请输入替换格式");
        return;
      }
      if (htmlFormat.isRegExp(this.replaceLableRegexInput)) {
        console.log("正则表达式");
        content = htmlFormat.replaceRegex(
          content,
          this.replaceLableRegexInput,
          this.replaceRichInput
        );
      } else {
        if (/^[a-zA-Z]/g.test(this.replaceLableRegexInput)) {
          content = htmlFormat.replaceLable(
            content,
            this.replaceLableRegexInput,
            this.replaceRichInput
          );
        } else {
          content = content.replaceAll(
            this.replaceLableRegexInput,
            this.replaceRichInput
          );
        }
      }
      message.success("内容替换成功");
      this.article.content = content;
      this.confirmLoading = false;
      this.formatVisible = false;
    },
  },
};
