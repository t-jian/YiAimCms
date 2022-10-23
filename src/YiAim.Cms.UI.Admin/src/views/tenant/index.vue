<template>
  <div class="app-container">
   
  </div>
</template>

<script>
// import {
//   getTenants,
//   deleteTenant
// } from '@/api/multi-tenancy/tenant'
// import Pagination from '@/components/Pagination' 
// import baseListQuery, { checkPermission } from '@/utils/abp'
// import TenantDialog from './components/tenant-dialog'
// import ConnectionstringDialog from './components/connectionstring-dialog'
// import FeatureDialog from './components/feature-dialog'

// export default {
//   name: 'Tenants',
//   components: {
//     Pagination,
//     TenantDialog,
//     ConnectionstringDialog,
//     FeatureDialog
//   },
//   data() {
//     return {
//       tableKey: 0,
//       list: null,
//       total: 0,
//       listLoading: true,
//       listQuery: baseListQuery
//     }
//   },
//   created() {
//     this.getList()
//   },
//   methods: {
//     checkPermission,
//     getList() {
//       this.listLoading = true
//       getTenants(this.listQuery).then(response => {
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
//     handleCreate() {
//       this.$refs['dialogDialog'].handleCreate()
//     },
//     handleUpdate(row) {
//       this.$refs['dialogDialog'].handleUpdate(row)
//     },
//     handleDelete(row, index) {
//       this.$confirm(
//         this.$i18n.t(
//           "AbpTenantManagement['TenantDeletionConfirmationMessage']",
//           [row.name]
//         ),
//         this.$i18n.t("AbpTenantManagement['AreYouSure']"),
//         {
//           confirmButtonText: this.$i18n.t("AbpTenantManagement['Yes']"),
//           cancelButtonText: this.$i18n.t("AbpTenantManagement['Cancel']"),
//           type: 'warning'
//         }
//       ).then(async() => {
//         deleteTenant(row.id).then(() => {
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
//     handleUpdateConnectionString(row) {
//       this.$refs['connectionstringDialog'].handleUpdate(row)
//     },
//     handleUpdateFeature(row) {
//       this.$refs['featureDialog'].handleUpdate(row)
//     }
//   }
// }
// </script>
