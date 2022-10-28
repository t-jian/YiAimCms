div<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button type="primary" size="small" plain icon="el-icon-plus" @click="batchAdd()"
        >批量添加</el-button
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

      <el-table-column label="缩略图" width="200px" align="center">
        <template slot-scope="{ row }">
          <div v-if="row.thumImg != ''" class="bg_thumb">
            <img
              :src="
                '/api/file/showbase64/' + encodeURIComponent(row.thumbMediaId)
              "
            />
          </div>
          <span v-if="row.thumImg == ''">无</span>
        </template>
      </el-table-column>

      <el-table-column label="标题" min-width="150px">
        <template slot-scope="{ row }">
          <span class="link-type">{{ row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column label="作者" width="80px" align="center">
        <template slot-scope="{ row }">
          <span>{{ row.author }}</span>
        </template>
      </el-table-column>
      <el-table-column label="添加时间" width="100px" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.createTime | parseTime("{m}-{d} {h}:{i}") }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        class-name="small-padding fixed-width"
        width="80px"
      >
        <template slot-scope="{row}">
          <el-button
            type="primary"
            icon="el-icon-plus"
            size="mini"
            @click="showDialog(row.index)"
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
      :page-sizes="[10, 20]"
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
        <el-button type="primary" size="small" @click="singleAdd()" > 确认 </el-button>
        <el-button size="small" @click="dialogFormVisible = false">
          取消
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import waves from "@/directive/waves";
import Pagination from "@/components/Pagination";
import article from "@/api/article";
import { GetAllColum } from "@/api/column";
export default {
  name: "ComplexTable",
  components: { Pagination },
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
      disabled: true,
      tableKey: 0,
      columnList: [],
      dialogFormVisible: false,
      currentChooseCat: 0,
      currentChooseIds: [],
    };
  },
  created() {
    this.getList();
  },
  methods: {
    getList() {
      this.listLoading = true;
      article.getPageWxNews(this.listQuery.page).then((response) => {
        this.list = response.items;
        this.total = response.totalCount;
        this.listLoading = false;
      });
    },
    showDialog(ids) {
       this.currentChooseIds=[]
       if(ids instanceof Array){
        this.currentChooseIds=ids
       }else{
        this.currentChooseIds.push(ids)
       }
      const loading = this.$loading({
        lock: true,
        text: "Loading",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.56)",
      });
      GetAllColum()
        .then((res) => {
          this.columnList = res;
          this.currentChooseCat = res[0].id;
          loading.close();
          this.dialogFormVisible = true;
        })
        .catch(() => {
          loading.close();
        });
    },
    singleAdd(){
     this.addNews()
    },
    addNews() {
      if (this.currentChooseIds.length > 0) {
           const loading = this.$loading({
        lock: true,
        text: "Loading",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.56)",
      });
        let ids = this.currentChooseIds.join(",")
        article
          .putWecahtOfficialNews(
            ids,
            this.currentChooseCat,
            this.listQuery.page,
            this.listQuery.limit
          )
          .then((res) => {
            loading.close()
            if(res.success){
                this.$message.success("添加成功")
                this.dialogFormVisible=false
                this.$router.push({path:'/cms/article'})
            }else{
             this.$message.error(res.message)
            }
          })
          .catch(() => {loading.close()});
      } else {
          this.$message.info("请选择项")
      }
    },
     selsChange(sels) {
      var ids = [];
      sels.forEach((item) => {
        ids.push(item.index);
      });
      this.sels = ids;
    },
    batchAdd(){
      if (this.sels.length < 1) {
        this.$message.info("请选择项！");
      }else{
        this.showDialog(this.sels)
      }
    }
  },
};
</script>
<style  scoped>
.bg_thumb {
  width: 180px;
  height: 90px;
}
.bg_thumb img {
  width: 100%;
  height: 100%;
  background-size: cover;
}
</style>
