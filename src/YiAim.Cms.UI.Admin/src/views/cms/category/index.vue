<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button
        type="primary"
        size="small"
        plain
        @click="showDialog('add')"
        icon="el-icon-edit"
        >新增分类</el-button
      >
    </div>
    <el-table
      v-loading="listLoading"
      :data="columnList"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row
    >
      <el-table-column align="center" label="ID" width="95">
        <template slot-scope="scope">
          {{ scope.row.id }}
        </template>
      </el-table-column>
      <el-table-column label="名称">
        <template slot-scope="scope">
          {{ scope.row.title }}
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        width="230"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="{ row }">
          <el-button
            type="primary"
            icon="el-icon-edit"
            size="mini"
            @click="updateShowDialog('update', row.id)"
          >
          </el-button>
          <el-button
            v-if="row.status != 'deleted'"
            icon="el-icon-delete"
            size="mini"
            type="danger"
            @click="deleteColumn(row.id)"
          >
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-dialog
      :title="textMap[dialogStatus]"
      :visible.sync="dialogFormVisible"
      width="400px"
    >
      <el-form
        ref="dataForm"
        :rules="rules"
        :model="temp"
        label-position="left"
        label-width="70px"
        style="width: 100%;margin: 0 auto"
      >
        <el-form-item label="名称" prop="title">
          <el-input v-model="temp.title" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button
          type="primary"
          size="small"
          @click="dialogStatus === 'add' ? createData() : updateData()"
        >
          确认
        </el-button>
        <el-button size="small" @click="dialogFormVisible = false">
          取消
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import {
  createCategory,
  deleteCategory,
  updateCategory,
  getAllCategory
} from "@/api/blogs/category";
export default {
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: "success",
        draft: "gray",
        deleted: "danger",
      };
      return statusMap[status];
    },
  },
  data() {
    return {
      listLoading: true,
      dialogFormVisible: false,
      temp: {
        title: "",
        taxis:0
      },
      textMap: {
        update: "编辑",
        add: "添加",
      },
      dialogStatus: "",
      rules: {
        title: [
          {
            required: true,
            message: "分类名称不能为空",
            trigger: "blur",
          },
        ],
      },
      columnList: [],
      value: "",
    };
  },
  created() {
    this.getColumnList();
  },
  methods: {
    getColumnList() {
      this.listLoading = true;
      getAllCategory()
        .then((res) => {
          this.columnList = res;
          this.listLoading = false;
        })
        .catch(() => {
          this.listLoading = false;
        });
    },
    updateShowDialog(type, id) {
      this.temp = this.columnList.filter((n) => n.id == id)[0];
      this.showDialog(type);
    },
    showDialog(type) {
      if (type == "add") {
        this.resetTemp();
      }
      this.dialogStatus = type;
      this.dialogFormVisible = true;
      this.$nextTick(() => {
        this.$refs["dataForm"].clearValidate();
      });
    },
    createData() {
      this.$refs["dataForm"].validate((valid) => {
        if (valid) {
          createCategory(this.temp).then((res) => {
              this.getColumnList()
              this.dialogFormVisible = false
              this.$mtip.success("添加成功")
            })
            .catch((res) => {
              this.dialogFormVisible = false
            });
        }
      });
    },
    updateData() {
      this.$refs["dataForm"].validate((valid) => {
        if (valid) {
          updateCategory(this.temp)
            .then((res) => {
              this.dialogFormVisible = false
              this.resetTemp()
              this.$mtip.success("修改成功")
            })
            .catch(() => {
              this.dialogFormVisible = false
            });
        }
      });
    },
    resetTemp() {
      this.temp = {
        id: undefined,
        title: "",
         taxis:0
      };
    },
    deleteColumn(id) {
      this.$mtip.delete(() => {
        deleteCategory(id)
          .then((res) => {
            this.getColumnList()
            this.$mtip.success("删除成功");
          })
      })
    },
  },
};
</script>
