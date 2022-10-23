<template>
  <div class="app-container">
   444
  </div>
</template>

// <script>
// import {
//   getRoles,
//   createRole,
//   getRoleById,
//   updateRole,
//   deleteRole
// } from '@/api/identity/role'
// import Pagination from '@/components/Pagination' // secondary package based on el-pagination
// import baseListQuery, { checkPermission } from '@/utils/abp'
// import PermissionDialog from './components/permission-dialog'

// export default {
//   name: 'Roles',
//   components: { Pagination, PermissionDialog },
//   data() {
//     return {
//       tableKey: 0,
//       list: null,
//       total: 0,
//       listLoading: true,
//       listQuery: baseListQuery,
//       temp: {
//         name: '',
//         isDefault: false,
//         isPublic: false
//       },
//       dialogFormVisible: false,
//       dialogStatus: '',
//       rules: {
//         name: [
//           {
//             required: true,
//             message: this.$i18n.t("AbpIdentity['The {0} field is required.']", [
//               this.$i18n.t("AbpIdentity['RoleName']")
//             ]),
//             trigger: 'blur'
//           },
//           {
//             max: 256,
//             message: this.$i18n.t(
//               "AbpIdentity['The field {0} must be a string with a maximum length of {1}.']",
//               [this.$i18n.t("AbpIdentity['RoleName']"), '256']
//             ),
//             trigger: 'blur'
//           }
//         ]
//       }
//     }
//   },
//   created() {
//     this.getList()
//   },
//   methods: {
//     checkPermission,
//     getList() {
//       this.listLoading = true
//       getRoles(this.listQuery).then(response => {
//         this.list = response.items
//         this.total = response.totalCount

//         this.listLoading = false
//       })
//     },
//     handleFilter(firstPage = true) {
//       if (firstPage) this.listQuery.page = 1
//       this.getList()
//     },
//     sortChange(data) {
//       const { prop, order } = data
//       this.listQuery.sort = order ? `${prop} ${order}` : undefined
//       this.handleFilter()
//     },
//     resetTemp() {
//       this.temp = {
//         name: '',
//         isDefault: false,
//         isPublic: false
//       }
//     },
//     handleCreate() {
//       this.resetTemp()
//       this.dialogStatus = 'create'
//       this.dialogFormVisible = true
//       this.$nextTick(() => {
//         this.$refs['dataForm'].clearValidate()
//       })
//     },
//     createData() {
//       this.$refs['dataForm'].validate(valid => {
//         if (valid) {
//           createRole(this.temp).then(() => {
//             this.handleFilter()
//             this.dialogFormVisible = false
//             this.$notify({
//               title: this.$i18n.t("HelloAbp['Success']"),
//               message: this.$i18n.t("HelloAbp['SuccessMessage']"),
//               type: 'success',
//               duration: 2000
//             })
//           })
//         }
//       })
//     },
//     handleUpdate(row) {
//       this.temp = Object.assign({}, row) // copy obj
//       this.dialogStatus = 'update'
//       this.dialogFormVisible = true

//       getRoleById(row.id).then(response => {
//         this.temp = response
//       })

//       this.$nextTick(() => {
//         this.$refs['dataForm'].clearValidate()
//       })
//     },
//     updateData() {
//       this.$refs['dataForm'].validate(valid => {
//         if (valid) {
//           updateRole(this.temp).then(() => {
//             this.handleFilter(false)
//             this.dialogFormVisible = false
//             this.$notify({
//               title: this.$i18n.t("HelloAbp['Success']"),
//               message: this.$i18n.t("HelloAbp['SuccessMessage']"),
//               type: 'success',
//               duration: 2000
//             })
//           })
//         }
//       })
//     },
//     handleDelete(row, index) {
//       this.$confirm(
//         this.$i18n.t("AbpIdentity['RoleDeletionConfirmationMessage']", [
//           row.name
//         ]),
//         this.$i18n.t("AbpIdentity['AreYouSure']"),
//         {
//           confirmButtonText: this.$i18n.t("AbpIdentity['Yes']"),
//           cancelButtonText: this.$i18n.t("AbpIdentity['Cancel']"),
//           type: 'warning'
//         }
//       ).then(async() => {
//         deleteRole(row.id).then(() => {
//           this.handleFilter()
//           this.$notify({
//             title: this.$i18n.t("HelloAbp['Success']"),
//             message: this.$i18n.t("HelloAbp['SuccessMessage']"),
//             type: 'success',
//             duration: 2000
//           })
//         })
//       })
//     },
//     handleUpdatePermission(row) {
//       this.$refs['permissionDialog'].handleUpdatePermission(row)
//     }
//   }
// }
// </script>
