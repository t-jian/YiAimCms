<template>
  <div class="login-container">
    <el-form
      ref="loginForm"
      :model="loginForm"
      :rules="loginRules"
      class="login-form"
      autocomplete="on"
      label-position="left"
    >
      <div class="title-container">
        <h3 class="title">
          {{ $t('Cms["Login"]') }}
        </h3>
        <lang-select class="set-language" />
      </div>

      <el-form-item prop="username">
        <span class="svg-container">
          <svg-icon icon-class="user" />
        </span>
        <el-input
          ref="username"
          v-model="loginForm.username"
          :placeholder="$t('AbpAccount[\'UserNameOrEmailAddress\']')"
          name="username"
          type="text"
          tabindex="1"
          autocomplete="on"
        />
      </el-form-item>

      <el-tooltip
        v-model="capsTooltip"
        content="Caps lock is On"
        placement="right"
        manual
      >
        <el-form-item prop="password">
          <span class="svg-container">
            <svg-icon icon-class="password" />
          </span>
          <el-input
            :key="passwordType"
            ref="password"
            v-model="loginForm.password"
            :type="passwordType"
            :placeholder="$t('AbpAccount[\'Password\']')"
            name="password"
            tabindex="2"
            autocomplete="on"
            @keyup.native="checkCapslock"
            @blur="capsTooltip = false"
            @keyup.enter.native="handleLogin"
          />
          <span class="show-pwd" @click="showPwd">
            <svg-icon
              :icon-class="passwordType === 'password' ? 'eye' : 'eye-open'"
            />
          </span>
        </el-form-item>
      </el-tooltip>

      <el-button
        :loading="loading"
        type="primary"
        style="width: 100%; margin-bottom: 30px"
        @click.native.prevent="handleLogin"
      >
        {{ $t("AbpAccount['Login']") }}
      </el-button>
      <div class="login-option-box">
        <el-tooltip content="使用Gitee进行登录" placement="top">
          <span @click="openAuth('gitee')"
            ><svg
              t="1611994252121"
              class="icon"
              viewBox="0 0 1024 1024"
              version="1.1"
              xmlns="http://www.w3.org/2000/svg"
              p-id="1398"
              width="32"
              height="32"
            >
              <path
                d="M512 1024C230.4 1024 0 793.6 0 512S230.4 0 512 0s512 230.4 512 512-230.4 512-512 512z m259.2-569.6H480c-12.8 0-25.6 12.8-25.6 25.6v64c0 12.8 12.8 25.6 25.6 25.6h176c12.8 0 25.6 12.8 25.6 25.6v12.8c0 41.6-35.2 76.8-76.8 76.8h-240c-12.8 0-25.6-12.8-25.6-25.6V416c0-41.6 35.2-76.8 76.8-76.8h355.2c12.8 0 25.6-12.8 25.6-25.6v-64c0-12.8-12.8-25.6-25.6-25.6H416c-105.6 0-188.8 86.4-188.8 188.8V768c0 12.8 12.8 25.6 25.6 25.6h374.4c92.8 0 169.6-76.8 169.6-169.6v-144c0-12.8-12.8-25.6-25.6-25.6z"
                fill="#888888"
                p-id="1399"
              ></path></svg
          ></span>
        </el-tooltip>
        <el-tooltip content="使用qq进行登录" placement="top">
          <span @click="openAuth('qq')">
            <svg
              t="1629872977396"
              class="icon"
              viewBox="0 0 1024 1024"
              version="1.1"
              xmlns="http://www.w3.org/2000/svg"
              p-id="4011"
              width="32"
              height="32"
            >
              <path
                d="M960 200.5c0-75.3-61.1-136.4-136.4-136.4H200.4c-75.3 0-136.4 61-136.4 136.4v623.1C64 898.9 125.1 960 200.4 960h623.1c75.3 0 136.4-61.1 136.4-136.4V200.5h0.1zM795 625.6V634.2l3.7 4.9 1.2 8.4-0.4 7.6-1.8 6.9-0.6 2.9-1.4 3.3-1.3 2.4-1.7 3-1.5 1.8-2 2.4-1.8 1.9-1.9 1.7-2.3 1-2-2.4-1.7-2.9h-2.4l-1.7 2.9-3.1 0.1-1.4-0.1-1.4-0.7-1.7-1.2-1.7-1.5-2.7-3-3.2-4.3-2.5-4.4-2.5-3.6-2.5-4-3.4-7.2-3.9-1.9-0.5 5.5h-0.6l-1.7-4.5-1-0.8-1.5 1.1-2.6 6.7-4 10.1-5.1 12.4-3.9 6.2-4 6.5-4.8 7.4-5.1 7.1-2.6 3.3-3.2 3.6-7.2 7.1 0.6 0.6 1 1.1 3.7 2.1 15.2 7.2 6.7 3.7 6.3 3.7 6.3 4.5 5.6 4.8 2.7 2.1 2 2.5 2 2.9 1.8 3.1 1 2.6 1.2 3.1 0.5 2.6-2.5 3.1-3.6 2.1v2l2.5 2.3 0.4 2 0.2 1.5-0.6 1.9-2.6 3.8-2.4 3-1.9 2.3-1.7 1.7-4.2 3.1-4.8 2.6-5.1 2.6-5.3 2.5-6.1 2.4-3.4 0.8-2.9 0.8-7 1.7-7.2 1.5-7.2 1.5-8 1.3-8.2 2.6-8.4 3.3H638.3l-9.4-2.9-8.8-1.9-9.4-1.9-9.4-1.3-9.9-1.5-9.5-2.5-9.3-1.9-9.3-2.5-9.5-3.3-9.3-2.9-4.9-1.7-4.4-1.5-2.7 0.8-2.7 1.4H522l-8.9-2.6-4.5-1.4-5.8-1.1-3.7 3.1-5.1 3.1-6.9 3.3-7.6 4.2-4.6 2.3-4.9 1.8-10.7 4.4-5.8 1.4-6.1 1.7-8.4 1.7-5.3 0.5-5.8 0.4-5.8 0.7-6.8 0.4h-27.8l-15.7-0.4-15.2-1.5-7.7-1.1-7.5-1.1-7.2-1.1-7.2-1.4-6.8-2.1-6.8-1.5-6.1-2.5-5.8-2.3-5.5-2.7-4.6-2.9-4.6-3.3-1.5-1.7-2-2-1.5-1.8-1.4-2-1.3-2-1.1-2-1.4-4.3 0.1-2.1v-17.5l0.2-2.7 1.2-3.1 1-3.1 1.9-3.7 1.3-1.4 1.1-1.7 3-3.6 2.3-1.9 2.1-1.3 2-1.7 3.2-1.1 2.5-1.5 3.2-1.7 3.7-1.1 3.7-1.1 4.2-1 4-0.6 4.8-0.6 5.1-1.3 1.3-1.4h0.2l-4.9 1.3v-0.4l1.8-1.3-1.3-1.3-6.2-5.9-4.2-3.7-5.1-4.6-5.3-5.2-5.5-6.7-6.3-7.5-2.4-4-3.2-4.2-2.7-4.9-2.5-5.2-3.3-5-2.1-5.7-2.5-5.7-2.6-6.4-1.9-6.1-1.8-9.5-0.6-2.3H264.4l-1.1 2.3-0.6 1.1-0.7 1.7-0.2 1.8-0.6 1.4-1 2.4-3.1 5.6-1.7 3.4-2.6 2.9-2.7 3.6-3 3.9-3.2 3.4-3.7 3.3-3.4 3.2-3.8 2.5-4.2 2.6-4 1.5-4.8 0.7-4.6-0.1h-1l-1.1 0.1-0.8-1.5-1.3-0.7-1.8-4.3-1.1-2.4-1.2-3.3-1-3.6-0.5-3.4-0.4-8 0.6-4.6V630.1l-0.6-11.4 0.4-5.8 0.8-5.9 0.8-6.4 1.7-6.1 1.9-6.9 2-6.8 2.7-6.9 2.5-6.7 3.6-6.8 3.3-7.2 4-6.9 4.8-7.4 4.8-6.7 5-7.4 4.2-5.1 5.3-5.9 5.6-5.9 2.6-2.9 3.2-3.2 4.6-4 4.8-4 7.7-6.9 5.8-4.3 2-1.5-1.2-3.6-6.2-4.6-5.8-2.6v-10.2l6.3-4 3.8-4.4 2.7-4.5 2.6-5.1 3.6-5.3 4.6-5.3v-3.7l-0.2-3.4 0.2-4.8 1.5-5.2 1.4-5.7 1.2-2.6 1.3-2.5-2.9-2.7-2.6-2V385.8l5.6-6.8 3.4-7.8 3.1-9.4 3.3-9.7 2.3-5.3 2.1-5.8 2.3-5.5 2.6-5.8 2.6-6.4 3.1-5.9 3.4-6.4 4.4-6.3 2-3.7 2.1-3 4.6-6.8 4.8-6.7 5.5-6.9 5.8-6.8 6.3-6.7 6.7-6.8 8-7.4 5.1-4.5 6.2-4.9 6.3-4.3 6.8-4.2 6.5-3.7 7.4-3 7.8-3.7 7.7-2.6 7.7-2.6 8.4-2.6 8.4-2 8.8-1.7 8.9-1.4 8.8-1.2 8.8-1.2 9.4-0.5H534.4l9.3 1.2 9.3 1.1 9.5 1 8.8 2.1 9.3 2 8.7 2.3 9.5 2.6 8.8 3.1 8.4 3.6 8.9 3.7 8.2 4 8 4.5 7.7 5 6.8 4.6 3.1 2.6 3.3 1.9 6.2 5.2 5.2 5.1 5.2 5.2 5.1 5.8 4.2 5.6 5 5.8 3.4 6.3 3.6 5.6 3.8 6.4 3.1 5.6 5.2 11.9 2.6 6.2 2 5.7 2 6.2 1.8 5.8 1.3 5 1.7 5.9 2.9 10.6 1.8 9.3 1.2 8.9 1.1 7.1 1.7 10.9 0.4 1.8 1.3 2 3.4 5.7 2.3 3.8 2 4 2.5 4.3 2.3 5.1 1.4 5.2 1.5 5.7 1.2 5.9-1.4 2.9-1.5 3.6v10l0.7 3.7-0.7 7.1v8.8l-0.6 1.3 1.3 3 8 12.1 6.3 9 3.1 6.2 4.2 6.7 3.6 7.2 4.2 7.8 4.2 8.8 4.8 10 2.6 6.1 2.5 5.9 2.1 6.3 2 5.6 1.5 5.9 1.7 5.6 2 10.8-2.9 11-3.3 9.9v5.7h0.2z"
                p-id="4012"
                fill="#888888"
              ></path>
              <path
                d="M960 200.5c0-75.3-61.1-136.4-136.4-136.4H200.4c-75.3 0-136.4 61-136.4 136.4v623.1C64 898.9 125.1 960 200.4 960h623.1c75.3 0 136.4-61.1 136.4-136.4V200.5h0.1zM795 625.6V634.2l3.7 4.9 1.2 8.4-0.4 7.6-1.8 6.9-0.6 2.9-1.4 3.3-1.3 2.4-1.7 3-1.5 1.8-2 2.4-1.8 1.9-1.9 1.7-2.3 1-2-2.4-1.7-2.9h-2.4l-1.7 2.9-3.1 0.1-1.4-0.1-1.4-0.7-1.7-1.2-1.7-1.5-2.7-3-3.2-4.3-2.5-4.4-2.5-3.6-2.5-4-3.4-7.2-3.9-1.9-0.5 5.5h-0.6l-1.7-4.5-1-0.8-1.5 1.1-2.6 6.7-4 10.1-5.1 12.4-3.9 6.2-4 6.5-4.8 7.4-5.1 7.1-2.6 3.3-3.2 3.6-7.2 7.1 0.6 0.6 1 1.1 3.7 2.1 15.2 7.2 6.7 3.7 6.3 3.7 6.3 4.5 5.6 4.8 2.7 2.1 2 2.5 2 2.9 1.8 3.1 1 2.6 1.2 3.1 0.5 2.6-2.5 3.1-3.6 2.1v2l2.5 2.3 0.4 2 0.2 1.5-0.6 1.9-2.6 3.8-2.4 3-1.9 2.3-1.7 1.7-4.2 3.1-4.8 2.6-5.1 2.6-5.3 2.5-6.1 2.4-3.4 0.8-2.9 0.8-7 1.7-7.2 1.5-7.2 1.5-8 1.3-8.2 2.6-8.4 3.3H638.3l-9.4-2.9-8.8-1.9-9.4-1.9-9.4-1.3-9.9-1.5-9.5-2.5-9.3-1.9-9.3-2.5-9.5-3.3-9.3-2.9-4.9-1.7-4.4-1.5-2.7 0.8-2.7 1.4H522l-8.9-2.6-4.5-1.4-5.8-1.1-3.7 3.1-5.1 3.1-6.9 3.3-7.6 4.2-4.6 2.3-4.9 1.8-10.7 4.4-5.8 1.4-6.1 1.7-8.4 1.7-5.3 0.5-5.8 0.4-5.8 0.7-6.8 0.4h-27.8l-15.7-0.4-15.2-1.5-7.7-1.1-7.5-1.1-7.2-1.1-7.2-1.4-6.8-2.1-6.8-1.5-6.1-2.5-5.8-2.3-5.5-2.7-4.6-2.9-4.6-3.3-1.5-1.7-2-2-1.5-1.8-1.4-2-1.3-2-1.1-2-1.4-4.3 0.1-2.1v-17.5l0.2-2.7 1.2-3.1 1-3.1 1.9-3.7 1.3-1.4 1.1-1.7 3-3.6 2.3-1.9 2.1-1.3 2-1.7 3.2-1.1 2.5-1.5 3.2-1.7 3.7-1.1 3.7-1.1 4.2-1 4-0.6 4.8-0.6 5.1-1.3 1.3-1.4h0.2l-4.9 1.3v-0.4l1.8-1.3-1.3-1.3-6.2-5.9-4.2-3.7-5.1-4.6-5.3-5.2-5.5-6.7-6.3-7.5-2.4-4-3.2-4.2-2.7-4.9-2.5-5.2-3.3-5-2.1-5.7-2.5-5.7-2.6-6.4-1.9-6.1-1.8-9.5-0.6-2.3H264.4l-1.1 2.3-0.6 1.1-0.7 1.7-0.2 1.8-0.6 1.4-1 2.4-3.1 5.6-1.7 3.4-2.6 2.9-2.7 3.6-3 3.9-3.2 3.4-3.7 3.3-3.4 3.2-3.8 2.5-4.2 2.6-4 1.5-4.8 0.7-4.6-0.1h-1l-1.1 0.1-0.8-1.5-1.3-0.7-1.8-4.3-1.1-2.4-1.2-3.3-1-3.6-0.5-3.4-0.4-8 0.6-4.6V630.1l-0.6-11.4 0.4-5.8 0.8-5.9 0.8-6.4 1.7-6.1 1.9-6.9 2-6.8 2.7-6.9 2.5-6.7 3.6-6.8 3.3-7.2 4-6.9 4.8-7.4 4.8-6.7 5-7.4 4.2-5.1 5.3-5.9 5.6-5.9 2.6-2.9 3.2-3.2 4.6-4 4.8-4 7.7-6.9 5.8-4.3 2-1.5-1.2-3.6-6.2-4.6-5.8-2.6v-10.2l6.3-4 3.8-4.4 2.7-4.5 2.6-5.1 3.6-5.3 4.6-5.3v-3.7l-0.2-3.4 0.2-4.8 1.5-5.2 1.4-5.7 1.2-2.6 1.3-2.5-2.9-2.7-2.6-2V385.8l5.6-6.8 3.4-7.8 3.1-9.4 3.3-9.7 2.3-5.3 2.1-5.8 2.3-5.5 2.6-5.8 2.6-6.4 3.1-5.9 3.4-6.4 4.4-6.3 2-3.7 2.1-3 4.6-6.8 4.8-6.7 5.5-6.9 5.8-6.8 6.3-6.7 6.7-6.8 8-7.4 5.1-4.5 6.2-4.9 6.3-4.3 6.8-4.2 6.5-3.7 7.4-3 7.8-3.7 7.7-2.6 7.7-2.6 8.4-2.6 8.4-2 8.8-1.7 8.9-1.4 8.8-1.2 8.8-1.2 9.4-0.5H534.4l9.3 1.2 9.3 1.1 9.5 1 8.8 2.1 9.3 2 8.7 2.3 9.5 2.6 8.8 3.1 8.4 3.6 8.9 3.7 8.2 4 8 4.5 7.7 5 6.8 4.6 3.1 2.6 3.3 1.9 6.2 5.2 5.2 5.1 5.2 5.2 5.1 5.8 4.2 5.6 5 5.8 3.4 6.3 3.6 5.6 3.8 6.4 3.1 5.6 5.2 11.9 2.6 6.2 2 5.7 2 6.2 1.8 5.8 1.3 5 1.7 5.9 2.9 10.6 1.8 9.3 1.2 8.9 1.1 7.1 1.7 10.9 0.4 1.8 1.3 2 3.4 5.7 2.3 3.8 2 4 2.5 4.3 2.3 5.1 1.4 5.2 1.5 5.7 1.2 5.9-1.4 2.9-1.5 3.6v10l0.7 3.7-0.7 7.1v8.8l-0.6 1.3 1.3 3 8 12.1 6.3 9 3.1 6.2 4.2 6.7 3.6 7.2 4.2 7.8 4.2 8.8 4.8 10 2.6 6.1 2.5 5.9 2.1 6.3 2 5.6 1.5 5.9 1.7 5.6 2 10.8-2.9 11-3.3 9.9v5.7h0.2z"
                p-id="4013"
                fill="#888888"
              ></path>
            </svg>
          </span>
        </el-tooltip>
        <el-tooltip content="使用Github进行登录" placement="top">
          <span @click="openAuth('github')"
            ><svg
              t="1628677048084"
              class="icon"
              viewBox="0 0 1024 1024"
              version="1.1"
              xmlns="http://www.w3.org/2000/svg"
              p-id="2461"
              width="32"
              height="32"
            >
              <path
                d="M511.957333 21.333333C241.024 21.333333 21.333333 240.981333 21.333333 512c0 216.832 140.544 400.725333 335.573334 465.664 24.490667 4.394667 32.256-10.069333 32.256-23.082667 0-11.690667 0.256-44.245333 0-85.205333-136.448 29.610667-164.736-64.64-164.736-64.64-22.314667-56.704-54.4-71.765333-54.4-71.765333-44.586667-30.464 3.285333-29.824 3.285333-29.824 49.194667 3.413333 75.178667 50.517333 75.178667 50.517333 43.776 75.008 114.816 53.333333 142.762666 40.789333 4.522667-31.658667 17.152-53.376 31.189334-65.536-108.970667-12.458667-223.488-54.485333-223.488-242.602666 0-53.546667 19.114667-97.322667 50.517333-131.669334-5.034667-12.330667-21.930667-62.293333 4.778667-129.834666 0 0 41.258667-13.184 134.912 50.346666a469.802667 469.802667 0 0 1 122.88-16.554666c41.642667 0.213333 83.626667 5.632 122.88 16.554666 93.653333-63.488 134.784-50.346667 134.784-50.346666 26.752 67.541333 9.898667 117.504 4.864 129.834666 31.402667 34.346667 50.474667 78.122667 50.474666 131.669334 0 188.586667-114.730667 230.016-224.042666 242.090666 17.578667 15.232 33.578667 44.672 33.578666 90.453334v135.850666c0 13.141333 7.936 27.605333 32.853334 22.869334C862.250667 912.597333 1002.666667 728.746667 1002.666667 512 1002.666667 240.981333 783.018667 21.333333 511.957333 21.333333z"
                p-id="2462"
                fill="#707070"
              ></path>
            </svg>
          </span>
        </el-tooltip>
      </div>
    </el-form>
  </div>
</template>

<script>
import { validUsername } from "@/utils/validate";
import LangSelect from "@/components/LangSelect";
import openWindow from "@/utils/open-window";
import {getAuthUrl}from "@/api/identity/auth"
export default {
  name: "Login",
  components: { LangSelect },
  data() {
    const validateUsername = (rule, value, callback) => {
      if (!validUsername(value)) {
        callback(new Error("Please enter the correct user name"));
      } else {
        callback();
      }
    };
    const validatePassword = (rule, value, callback) => {
      if (value.length < 6) {
        callback(new Error("The password can not be less than 6 digits"));
      } else {
        callback();
      }
    };
    return {
      loginForm: {
        username: "admin",
        password: "111111",
      },
      loginRules: {
        username: [
          {
            required: true,
            trigger: "blur",
            message: this.$i18n.t("AbpAccount['ThisFieldIsRequired.']"),
          },
        ],
        password: [
          {
            required: true,
            trigger: "blur",
            essage: this.$i18n.t("AbpAccount['ThisFieldIsRequired.']"),
          },
        ],
      },
      passwordType: "password",
      capsTooltip: false,
      loading: false,
      showDialog: false,
      redirect: undefined,
      otherQuery: {},
    };
  },

  created() {
    window.addEventListener("storage", this.afterQRScan);
  },
  destroyed() {
    window.removeEventListener("storage", this.afterQRScan);
  },
  watch: {
    $route: {
      handler: function (route) {
        const query = route.query;
        if (query) {
          this.redirect = query.redirect;
          this.otherQuery = this.getOtherQuery(query);
        }
      },
      immediate: true,
    },
  },
  mounted() {
    if (this.loginForm.username === "") {
      this.$refs.username.focus();
    } else if (this.loginForm.password === "") {
      this.$refs.password.focus();
    }
  },

  methods: {
  openAuth(type) {
      if (window.localStorage) {
        window.localStorage.setItem("authtype", type);
      }
      getAuthUrl(type).then((res) => {
          openWindow(res, type, 860, 540);
        })
        
    },
    checkCapslock(e) {
      const { key } = e;
      this.capsTooltip = key && key.length === 1 && key >= "A" && key <= "Z";
    },
    showPwd() {
      if (this.passwordType === "password") {
        this.passwordType = "";
      } else {
        this.passwordType = "password";
      }
      this.$nextTick(() => {
        this.$refs.password.focus();
      });
    },
    handleLogin() {
      this.$refs.loginForm.validate((valid) => {
        if (valid) {
          this.loading = true;
          let clientSetting = {
              grant_type: "password",
              scope: "Cms",
              username: this.loginForm.username.trim(),
              password: "1q2w3E*",
              client_id: "Cms_App",
              client_secret: ""
         }
          this.$store.dispatch("user/login", clientSetting)
            .then(() => {
              this.$router.push({
                path: this.redirect || "/",
                query: this.otherQuery,
              });
              this.loading = false;
            })
            .catch(() => {
              this.loading = false;
            });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    getOtherQuery(query) {
      return Object.keys(query).reduce((acc, cur) => {
        if (cur !== "redirect") {
          acc[cur] = query[cur];
        }
        return acc;
      }, {});
    },
    afterQRScan(e) {
      if (e.key === "x-admin-oauth-code") {
        this.loading = true;
        let codeState = e.newValue.split('&');
        let type = e.storageArea.authtype;
        let clientSetting = {
          grant_type: "third_auth",
          scope: "Cms",
          code: codeState[0].split('=')[1],
          state:codeState[1].split('=')[1],
          client_id: "third_auth",
          type:type,
          client_secret: ""
      }
      console.log(clientSetting,555)
        this.$store.dispatch("user/login", clientSetting)
            .then(() => {
              this.$router.push({
                path: this.redirect || "/",
                query: this.otherQuery,
              });
              this.loading = false;
            })
            .catch(() => {
              this.loading = false;
            });
      }
    },
  },
};
</script>

<style lang="scss">
/* 修复input 背景不协调 和光标变色 */
/* Detail see https://github.com/PanJiaChen/vue-element-admin/pull/927 */

$bg: #283443;
$light_gray: #fff;
$cursor: #fff;

@supports (-webkit-mask: none) and (not (cater-color: $cursor)) {
  .login-container .el-input input {
    color: $cursor;
  }
}

/* reset element-ui css */
.login-container {
  .el-input {
    display: inline-block;
    height: 47px;
    width: 85%;

    input {
      background: transparent;
      border: 0px;
      -webkit-appearance: none;
      border-radius: 0px;
      padding: 12px 5px 12px 15px;
      color: $light_gray;
      height: 47px;
      caret-color: $cursor;

      &:-webkit-autofill {
        box-shadow: 0 0 0px 1000px $bg inset !important;
        -webkit-text-fill-color: $cursor !important;
      }
    }
  }

  .el-form-item {
    border: 1px solid rgba(255, 255, 255, 0.1);
    background: rgba(0, 0, 0, 0.1);
    border-radius: 5px;
    color: #454545;
  }
}
</style>

<style lang="scss" scoped>
$bg: #2d3a4b;
$dark_gray: #889aa4;
$light_gray: #eee;

.login-container {
  min-height: 100%;
  width: 100%;
  background-color: $bg;
  overflow: hidden;

  .login-form {
    position: relative;
    width: 520px;
    max-width: 100%;
    padding: 160px 35px 0;
    margin: 0 auto;
    overflow: hidden;
  }

  .tips {
    font-size: 14px;
    color: #fff;
    margin-bottom: 10px;

    span {
      &:first-of-type {
        margin-right: 16px;
      }
    }
  }

  .svg-container {
    padding: 6px 5px 6px 15px;
    color: $dark_gray;
    vertical-align: middle;
    width: 30px;
    display: inline-block;
  }

  .title-container {
    position: relative;

    .title {
      font-size: 26px;
      color: $light_gray;
      margin: 0px auto 40px auto;
      text-align: center;
      font-weight: bold;
    }

    .set-language {
      color: #fff;
      position: absolute;
      top: 3px;
      font-size: 18px;
      right: 0px;
      cursor: pointer;
    }
  }

  .show-pwd {
    position: absolute;
    right: 10px;
    top: 7px;
    font-size: 16px;
    color: $dark_gray;
    cursor: pointer;
    user-select: none;
  }

  .thirdparty-button {
    position: absolute;
    right: 0;
    bottom: 6px;
  }

  @media only screen and (max-width: 470px) {
    .thirdparty-button {
      display: none;
    }
  }
   .login-option-box {
        display: flex;
        justify-items: center;
        justify-content: center;
        span {
            margin-right: 10px;
            cursor: pointer;
            svg {
                width: 26px;
                height: 26px;
            }
        }
    }
}
</style>
