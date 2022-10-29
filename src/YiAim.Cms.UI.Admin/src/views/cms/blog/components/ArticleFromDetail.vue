<template>
  <div class="app-main-container" v-show="isEditorInit">
    <div
      class="createPost-container appmsg_input_area"
      v-loading="confirmLoading"
    >
      <el-form ref="postForm"  class="form-container">
        <div class="createPost-container news-form-containre">
          <div id="js_title_main" class="js_title_main input_box" prop="title">
            <input
              id="title"
              type="text"
              v-model="article.title"
              placeholder="请在这里输入标题"
              class="frm_input title"
              name="title"
              @focus="titleFocus"
              @blur="blur"
            />
            <em
              :class="
                calcTitleLength > titleMaxLen
                  ? 'frm_counter warn'
                  : 'frm_counter'
              "
              style="right: -40px"
              v-show="isTitleWordCount"
              >{{ calcTitleLength }}/{{ titleMaxLen }}</em
            >
          </div>
          <div class="content-tip" v-show="isShowContentTip">
            正文从这里开始
          </div>
          <ueditor
            v-model="article.content"
            :config="editorConfig"
            :editor-dependencies="editorDependencies"
            :destroy="true"
            @ready="readyUeditor"
          ></ueditor>

          <div class="article_setting_area">
            <div id="js_cover_description_area">
              <div class="js_cover_description_area_bar">
                <span>封面和摘要</span>
              </div>
              <div class="setting-group__content">
                <div class="setting-group__cover">
                  <div class="thumb-box">
                    <ul class="el-upload-list el-upload-list--picture-card">
                      <li v-for="item in thumbList" :key="item">
                        <div class="el-upload--picture-card">
                          <img
                            class="el-upload-list__item-thumbnail"
                            :src="item.url"
                            alt=""
                          />
                          <span class="el-upload-list__item-actions">
                            <span class="el-upload-list__item-preview">
                              <i class="el-icon-zoom-in"></i>
                            </span>
                            <span class="el-upload-list__item-delete">
                              <i class="el-icon-delete"></i>
                            </span>
                          </span>
                        </div>
                      </li>
                      <li>
                        <div class="upload-panel">
                          <div class="el-upload el-upload--picture-card">
                            <i class="el-icon-plus"></i>
                            <div class="upload-option">
                              <div class="item">
                                <span @click="getDetailImgs">从正文中选择</span>
                              </div>
                              <div class="item">
                                <el-upload
                                 action="#"
                                  :before-upload="beforeUpload"
                                  :http-request="upload"
                                >
                                  <span style="font-size: 12px"
                                    >选择图片上传</span
                                  >
                                </el-upload>
                              </div>
                            </div>
                          </div>
                        </div>
                      </li>
                    </ul>
                  </div>
                </div>
                <div style="width: 100%">
                  <span class="frm_textarea_box js_description_span">
                    <textarea
                      id="js_description"
                      v-model="article.digest"
                      placeholder="选填 摘要 如不填写则默认抓取正文前160字"
                      class="frm_textarea"
                      name="digest"
                    ></textarea>
                    <em
                      id="desc_frm_counter"
                      :class="
                        calcDescLength > descMaxLen
                          ? 'frm_counter warn'
                          : 'frm_counter'
                      "
                      >{{ calcDescLength }}/{{ descMaxLen }}</em
                    >
                  </span>
                </div>
              </div>
            </div>
          </div>
          <div class="mrow">
            文章设置
            <span style="font-size: 12px; color: #999">(分类、发布时间)</span>
          </div>
          <div
            class="mrow"
            style="display: flex; justify-content: space-between"
          >
          <el-row :gutter="20">
            <el-col :span="8">
                  <el-select v-model="article.categoryId" placeholder="选择分类">
              <el-option
                v-for="item in categories"
                :key="item.id"
                :label="item.title"
                :value="item.id"
              >
              </el-option>
            </el-select> 
            </el-col>
              <el-col :span="8">
                <el-date-picker
              v-model="article.publishDate"
              type="datetime"
              placeholder="发布时间"
            ></el-date-picker>
              </el-col>
                <el-col :span="8">
                   <el-input placeholder="作者" v-model="article.author" />
                </el-col>
          </el-row>
          </div>
          <div>合集标签</div>
        <div class="article_tag_area">
            <el-tag
              :key="tag"
              v-for="tag in dynamicTags"
              closable
              :disable-transitions="false"
              @close="handleClose(tag)"
            >
              {{ tag }}
            </el-tag>
            <el-input
              class="input-new-tag"
              v-if="inputVisible"
              v-model="inputValue"
              ref="saveTagInput"
              size="small"
              @keyup.enter.native="handleInputConfirm"
            >
            </el-input>
            <el-button
              v-else
              class="button-new-tag"
              size="small"
              @click="showInput"
              >添加标签</el-button
            >
          </div>

          <div class="gran-box"></div>
          <div class="footer">
           <div class="js_bot_bar tool_area">
              <el-button
                type="primary" plain
                @click="showDrawer"
                style="position: absolute; top: 20px; left: 22px;font-size:12px"
                >清除/替换格式</el-button
              >
              <el-button
                type="primary"
                style="position: absolute; top: 20px; left: 142px;font-size:12px"
                @click="rollback"
                >返回</el-button
              >
              <template v-if="isEdit">
                <template v-if="article.status == 1">
                  <el-button type="success" @click="submitForm(1)">{{ submitTxts[1] }}</el-button>
                </template>
                <template v-else>
                  <el-button type="success" @click="submitForm(2)">保存为草稿</el-button>
                  <el-button type="success" @click="submitForm(1)">{{ submitTxts[0] }}</el-button>
                </template>
              </template>
              <template v-else>
                <el-button type="success" @click="submitForm(2)">保存为草稿</el-button>
                <el-button type="success" @click="submitForm(1)">{{ submitTxts[0] }}</el-button>
              </template>
            </div>
          </div>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script>
import articleContaller from "@/assets/js/articleEdit.js";
export default {
  ...articleContaller,
};
</script>

<style scoped>
@import "~@/styles/articleEdit.css";
</style>
