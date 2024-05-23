export const chunksOptions: { [chunkAlias: string]: string[] } = {

    'chunk_shared': [
        '@/views/_shared/login/LoginView.vue',
        '@/views/_shared/settings/SettingsView.vue',
        '@/views/_shared/not-found/NotFoundView.vue',
        '@/views/_shared/supported-pattern-formats/SupportedPatternFormatsView.vue',
    ],

    'chunk_admin': [
        '@/views/admin/admin/AdminView.vue',
        '@/views/admin/admin/AdminView.vue',
    ],

    'chunk_creator': [
        '@/views/creator/dashboard/CreatorDashboardView.vue',
        '@/views/creator/patterns/CreatorPatternsView.vue',
        '@/views/creator/new-pattern/NewPatternView.vue',
    ],

    'chunk_stitcher': [
        '@/views/stitcher/about/AboutView.vue',
        '@/views/stitcher/dashboard/DashboardView.vue',
        '@/views/stitcher/new-pattern/NewPatternView.vue',
        '@/views/stitcher/patterns/PatternsView.vue',
        '@/views/stitcher/project/ProjectView.vue',
        '@/views/stitcher/tools/ToolsView.vue',
        '@/views/stitcher/project-analytics/ProjectAnalyticsView.vue',
        '@/views/stitcher/inventory/InventoryView.vue',
        '@/views/stitcher/creator/CreatorView.vue',
        '@/views/_shared/signup/SignupView.vue',
        '@/views/_shared/marketing/MarketingView.vue',
    ],

};