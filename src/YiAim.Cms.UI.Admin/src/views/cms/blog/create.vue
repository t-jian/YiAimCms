<template>
  <article-detail :is-edit="false" />
</template>

<script>
import ArticleDetail from './components/ArticleFromDetail'

export default {
  name: 'CreateArticle',
  components: { ArticleDetail }
}
</script>

 