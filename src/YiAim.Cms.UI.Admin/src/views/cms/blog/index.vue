<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button
        type="primary"
        size="small"
        plain
        @click="handleCreate"
        icon="el-icon-edit"
        >添加文章</el-button
      >
      <el-button
        type="primary"
        size="small"
        plain
        @click="addWxNews"
        icon="el-icon-edit"
        >添加公众号文章</el-button
      >
      <el-button
        type="warning"
        size="small"
        icon="el-icon-delete"
        @click="handleMultiDel"
        >批量删除</el-button
      >
    </div>
    <el-table
      v-loading="listLoading"
      :data="list"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row
      @selection-change="selsChange"
    >
      <el-table-column type="selection" width="40"></el-table-column>

      <!-- <el-table-column label="缩略图" width="200px" align="center">
        <template slot-scope="{ row }">
          <span v-if="row.thumImg == ''">无</span>
          <div v-else>
            <div v-if="row.isWx" class="bg_thumb bg_thumb_wx">
              <img
                v-if="row.thumImg.indexOf('mmbiz.qpic.cn') > -1"
                :src="'/api/file/showbase64/' + encodeURIComponent(row.thumImg)"
              />
              <img v-else :src="row.thumImg" />
            </div>
            <div v-else class="bg_thumb">
              <img :src="row.thumImg" />
            </div>
          </div>
        </template>
      </el-table-column> -->

      <el-table-column label="标题" min-width="150px">
        <template slot-scope="{ row }">
          <span class="link-type">{{ row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column label="分类" width="110px" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="showDialog(scope.row.id)"
            >{{getCatTitle(scope.row.categoryId)}}
            <i class="el-icon el-icon-edit"></i
          ></span>
        </template>
      </el-table-column>
      <el-table-column label="作者" width="80px" align="center">
        <template slot-scope="{ row }">
          <span>{{ row.author }}</span>
        </template>
      </el-table-column>
      <el-table-column label="排序" width="80px" align="center">
        <template slot-scope="{ row }">
          <span>{{ row.taxis }}</span>
        </template>
      </el-table-column>

      <!-- <el-table-column label="添加时间" width="100px" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.addTime | parseTime("{m}-{d} {h}:{i}") }}</span>
        </template>
      </el-table-column> -->
      <el-table-column
        label="操作"
        align="center"
        class-name="small-padding fixed-width"
        width="150px"
      >
        <template slot-scope="{ row }">
          <el-button
            type="primary"
            icon="el-icon-edit"
            size="mini"
            title="编辑"
            @click="handleUpdate(row.id)"
          >
          </el-button>
          <el-button
            type="success"
            size="mini"
            title="更换封面"
            icon="el-icon-picture-outline"
            @click="upArticleThumb(row.id)"
          >
          </el-button>
          <el-button
            v-if="row.status != 'deleted'"
            icon="el-icon-delete"
            size="mini"
            type="danger"
            title="删除"
            @click="handleDelete(row.id)"
          >
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="listQuery.page"
      :limit.sync="listQuery.limit"
      @pagination="getList"
    />

    <el-dialog
      title="选项归属分类"
      :visible.sync="dialogFormVisible"
      width="400px"
    >
      <el-form>
        <el-form-item label="分类">
          <el-select placeholder="请选择" v-model="currentChooseCat">
            <el-option
              v-for="item in columnList"
              :key="item.id"
              :label="item.title"
              :value="item.id"
            >
            </el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" size="small" @click="editCat()">
          确认
        </el-button>
        <el-button size="small" @click="dialogFormVisible = false">
          取消
        </el-button>
      </div>
    </el-dialog>
    <!-- <image-cropper
      ref="imagecropper"
      :options="cropperOptions"
      :imgSize="3"
      :imgType="imgType"
      :imageUrl="imgUrl"
      @crop-upload-close="cropClose"
      @crop-upload-success="cropSuccess"
    /> -->
  </div>
</template>

<script>
import waves from "@/directive/waves";
import Pagination from "@/components/Pagination";
import { deleteBlog, getPage, updateTaxis,batchDeleteIds } from "@/api/blogs/blog";
import { getAllCategory } from "@/api/blogs/category";
import ImageCropper from "@/components/ImageCropper";
export default {
  name: "ComplexTable",
  components: { Pagination, ImageCropper },

  directives: { waves },
  data() {
    return {
      total: 0,
      list: null,
      listLoading: true,
      listQuery: {
        page: 1,
        limit: 10,
      },
      sels: [],
      dialogFormVisible: false,
      currentChooseCat: 0,
      columnList: [],
      currentChooseArticleId: null,
      cropperOptions: {
        autoCrop: true, //是否默认生成截图框
        autoCropWidth: 554, //默认生成截图框宽度
        autoCropHeight: 236, //默认生成截图框高度
        fixedNumber: [2.35, 1],
        fixedBox: false,
        canScale: false,
        fiexd: true,
        full: true,
        infoTrue: false,
        centerBox: true, //截图框是否被限制在图片里面
        previewsCircle: false, //预览图是否是圆形
        title: "上传封面",
      },
      imgType: "testUp",
      imgUrl: "",
      currentEditArticleId: null,
    };
  },
  created() {
    Promise.all([getAllCategory(), this.getList()]).then((res) => {
      let cates = res[0];
      if (cates != null) {
        this.columnList=cates
      }
    });
  },
  methods: {
    upArticleThumb(id) {
      this.currentEditArticleId = id;
      this.$refs.imagecropper.clickShowUpload();
    },

    //上传操作结束
    cropClose() {
      console.log("上传操作结束");
    },
    //上传图片成功
    cropSuccess(data, callback) {
      console.log(data, "上传图片成功");
      if (this.currentEditArticleId != null) {
        article
          .updateThumb({ id: this.currentEditArticleId, path: data.fileUrl })
          .then((res) => {
            callback(false);
            if (res.success) {
              let currentArticle = this.list.filter(
                (n) => n.id == this.currentEditArticleId
              )[0];
              currentArticle.thumImg = data.fileUrl;
              this.$mtip.success(res.message);
            } else {
              this.$mtip.error(res.message);
            }
          })
          .catch((error) => {
            callback(false);
          });
      } else {
        callback(false);
      }
    },

    addWxNews() {
      this.$router.push({ path: "/cms/wxNews" });
    },
    getList() {
      this.listLoading = true;
      getPage(this.listQuery).then((response) => {
        this.list = response.items;
        this.total = response.totalCount;
        this.listLoading = false;
      });
    },
    selsChange(sels) {
      var ids = [];
      sels.forEach((item) => {
        ids.push(item.id);
      });
      this.sels = ids;
    },
    handleMultiDel() {
      if (this.sels.length < 1) {
        this.$message.info("请选择要删除的项！");
      }
      let loading = this.$mtip.loading();
     batchDeleteIds(this.sels.join(","))
        .then((res) => {
          this.$mtip.success('删除成功')
          this.getList();
          loading.close();
        })
        .catch((error) => {
          loading.close();
        });
    },
    handleCreate() {
      this.$router.push({ path: "/cms/create" });
    },
    handleUpdate(id) {
      this.$router.push({ path: "/cms/edit/" + id });
    },
    handleDelete(id) {
      var _this = this;
      this.$mtip.delete(() => {
        let loading = this.$mtip.loading();
        deleteBlog(id)
          .then(() => {
            this.$message.success("删除成功")
            _this.getList();
            loading.close();
          })
          .catch((error) => {
            loading.close();
          });
      });
    },
    showDialog(id) {
      this.currentChooseArticleId = id;
      this.currentChooseCat = this.list.filter((n) => n.id == id)[0].categoryId;
      let loading = this.$mtip.loading();
      if (this.currentChooseCat == undefined) {
         this.currentChooseCat = this.columnList[0].id;
      }
      loading.close();
      this.dialogFormVisible = true;
    },
    editCat() {
      if (this.currentChooseArticleId != null) {
        let loading = this.$mtip.loading();
        updateTaxis(this.currentChooseArticleId, this.currentChooseCat)
          .then((res) => {
            let currentArticle = this.list.filter(
              (n) => n.id == this.currentChooseArticleId
            )[0];
            let curCat = this.columnList.filter(
              (n) => n.id == this.currentChooseCat
            )[0];
            currentArticle.categoryId = curCat.id;
            this.$mtip.success("更新成功");
            this.dialogFormVisible = false;
            loading.close();
          })
          .catch((error) => {
            console.log(error);
            loading.close();
          });
      }
    },
    getCatTitle(id){
        let index=this.columnList.findIndex(n=>n.id==id);
        if(this.columnList.findIndex(n=>n.id==id)>-1){
            return this.columnList[index].title
        }
        return "无"
    }
  },
};
</script>
<style scoped>
.fixed-width .el-button--mini {
  padding: 7px 10px;
  min-width: 30px;
}

</style>
