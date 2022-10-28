
import Layout from "@/layout";

const tenantRouter = {
  path: "/tenant",
  component: Layout,
  redirect: "/tenant/tenants",
  alwaysShow: true,
  name: "Tenant",
  meta: {
    title: 'AbpTenantManagement["Menu:TenantManagement"]',
    icon: "tree"
  },
  children: [
    {
      path: "tenants",
      component: () => import("@/views/tenant/index"),
      name: "Tenants",
      meta: { title:  'AbpTenantManagement["Tenants"]', policy: "AbpTenantManagement.Tenants" }
    }
  ]
};
export default tenantRouter;
