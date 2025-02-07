import { createRouter, createWebHistory } from 'vue-router'
import TimeTrackingPage from "../views/TimeTrackingPage.vue";
import TopicOverviewPage from "@/views/TopicOverviewPage.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'TimeTracking',
      component: TimeTrackingPage
    },
    {
      path: '/topics',
      name: 'Topics',
      component: TopicOverviewPage
    }
  ]
})

export default router
