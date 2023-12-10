<template>
    <ViewComponent class="login-view" hide-nav>
        <div class="content-width">
            <h1 class="flex gap-small align-items-center">
                <div class="logo-container flex-auto">
                    <img width="" src="@/assets/logo.png">
                </div>
                <span>ClickStitch</span>
            </h1>
            <ContentCardComponent>
                <h2><IconComponent icon="user" size="large" gap="right" />Log In</h2>
                <p>
                    <label>
                        <strong>Email</strong>
                        <br>
                        <input ref="emailInput" type="text" v-model="email" placeholder="Email" @keyup.enter="nextInput('passwordInput')">
                    </label>
                </p>
                <p>
                    <label>
                        <strong>Password</strong>
                        <br>
                        <input ref="passwordInput" type="password" v-model="password" placeholder="Password" @keyup.enter="nextInput('emailInput')">
                    </label>
                </p>
                <UserMessageComponent ref="userMessageComponent" />
                <ButtonComponent class="tertiary" @click="onLogin" :loading="isLoading">Log In</ButtonComponent>
                <p>
                    Don't have an account? <RouterLink to="/signup"><ButtonComponent class="mini">Sign Up</ButtonComponent></RouterLink>
                </p>
            </ContentCardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

import ContentCardComponent from '@/views/_shared/login/components/ContentCard.component.vue';
import UserMessageComponent from '@/components/UserMessage.component.vue';

import { api } from '@/api/api';

import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();
const router = useRouter();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const emailInput = ref<HTMLInputElement>({} as HTMLInputElement);
const passwordInput = ref<HTMLInputElement>({} as HTMLInputElement);

const inputs = {
    emailInput,
    passwordInput,
};

const email = ref<string>('');
const password = ref<string>('');

const isLoading = ref<boolean>(false);

const nextInput = async function (next: 'emailInput' | 'passwordInput'): Promise<void> {
    const input = inputs[next].value;
    if (input.value.length === 0) {
        input.focus();
        return;
    }

    await onLogin();
};

const onLogin = async function () {
    if (isLoading.value)
        return;

    userMessageComponent.value.clear();

    if (email.value.length === 0) {
        userMessageComponent.value.set('Please enter your email.');
        return;
    }
    if (password.value.length === 0) {
        userMessageComponent.value.set('Please enter your password.');
        return;
    }

    isLoading.value = true;

    const result = await api.auth.logIn({
        email: email.value,
        password: password.value,
    });

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message);
        return;
    }

    auth.set(result);

    await router.push({ path: '/dashboard' });
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.login-view {

    .content-width {
        max-width: 720px;
    }

    .logo-container {
        width: 40px;
        height: 40px;
        padding: 0.25rem;
        background-color: var(--wjb-light);
        border-radius: 50%;

        @include shadow-small();

        img {
            max-width: 100%;
        }
    }

    input {
        width: 100%;
    }
}
</style>