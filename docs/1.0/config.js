let config = {
    title: 'Native SDK文档',
    home: 'Home.md',
    repo: 'Jie2GG/Native.Framework',
    nav: [
        {
            title: '首页', path: '/'
        },
        {
            title: '开始使用', type: 'dropdown', items: [
				{
					title: '快速入门', path: '/QuickStart'
				},
				{
					title: '应用配置', path: '/Configuration'
				},
				{
					title: '实现接口', path: '/Register'
				},
				{
					title: '编译部署', path: '/Build'
				},
				{
					title: '应用调试', path: '/Debug'
				}
			]
        },
        {
            title: '进阶使用', type: 'dropdown', items: [
                {
                    title: '增加窗体', path: '/Window'
                },
				{
                    title: '增加悬浮窗', path: '/Flow'
                }
			]
        },
        {
            title: '版本升级', path: '/Upgrade'
        },
        {
            title: '其它', type: 'dropdown', items: [
                {
                    title: 'Docker', path: '/Docker'
                },
                {
                    title: '扩展', path: '/Extension'
                },
                {
                    title: '常见问题', path: '/Issues'
                }
            ]
        }
    ],
    tocVisibleDepth: 2,
    plugins: []
};