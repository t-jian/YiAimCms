import Vue from 'vue'
import VueI18n from 'vue-i18n'
import Cookies from 'js-cookie'
import elementEnLocale from 'element-ui/lib/locale/lang/en' // element-ui lang
import elementZhLocale from 'element-ui/lib/locale/lang/zh-CN'// element-ui lang

Vue.use(VueI18n)

const messages = {
  en: {
    ...elementEnLocale
  },
  'zh-CN': {
    ...elementZhLocale
  }
}

export function setLocale(language, values) {
  i18n.mergeLocaleMessage(language, values)
  i18n.locale = language
}
export function getLanguage() {
  const chooseLanguage = Cookies.get('language')
  if (chooseLanguage) return chooseLanguage
  const language = (
    navigator.language || navigator.browserLanguage
  ).toLowerCase()
  const locales = Object.keys(messages)
  for (const locale of locales) {
    if (language.indexOf(locale) > -1) {
      return locale
    }
  }
  return 'zh-Hans'
}
const i18n = new VueI18n({
  locale: getLanguage(),
  messages
})

export default i18n
