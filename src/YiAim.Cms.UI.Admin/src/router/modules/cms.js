import Layout from "@/layout";

const identityRouter = 
    {
        path: '/cms',
        component: Layout,
        redirect: '/cms/blog',
        name: '内容管理',
        meta: { title: '内容管理', icon: 'el-icon-s-help' },
        children: [{
                path: 'blog',
                name: '文章',
                component: () =>
                    import ('@/views/cms/blog/index'),
                meta: { title: '文章', icon: 'table', activeMenu: '/cms/blog' }
            },
            {
                path: 'wxNews',
                name: '公众号文章',
                component: () =>
                    import ('@/views/cms/blog/wxNews'),
                meta: { title: '公众号文章', icon: 'table', activeMenu: '/cms/blog' },
                hidden: true
            },
            {
                path: 'create',
                component: () =>
                    import ('@/views/cms/blog/create'),
                name: 'CreateArticle',
                meta: { title: 'Create Article', icon: 'edit', activeMenu: '/cms/blog' },
                hidden: true
            },
            {
                path: 'edit/:id(\\d+)',
                component: () =>
                    import ('@/views/cms/blog/edit'),
                name: 'EditArticle',
                meta: { title: 'editArticle', noCache: true, activeMenu: '/cms/blog' },
                hidden: true
            },
            {
                path: 'tree',
                name: '分类',
                component: () =>
                    import ('@/views/cms/category/index'),
                meta: { title: '分类', icon: 'tree' }
            },
            {
                path: 'tags',
                name: '标签',
                meta: { title: '标签', icon: 'tree' },
                component: () =>
                    import ('@/views/cms/tag/index'),
                hidden: true
            }
        ]
    };
export default identityRouter;