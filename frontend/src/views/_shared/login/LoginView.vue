<template>
    <ViewComponent class="login-view" hide-nav>
        <div class="mx-auto px-4 max-w-[720px]">
            <ContentCardComponent>
                <h2><IconComponent icon="user" size="large" gap="right" />Log In</h2>
                <label>
                    <strong>Email</strong>
                    <input ref="emailInput" type="text" v-model="email" placeholder="Email" @keyup.enter="nextInput('passwordInput')" class="w-full">
                </label>
                <label class="my-4 block">
                    <strong>Password</strong>
                    <br>
                    <input ref="passwordInput" type="password" v-model="password" placeholder="Password" @keyup.enter="nextInput('emailInput')" class="w-full">
                </label>
                <UserMessageComponent ref="userMessageComponent" />
                <BtnComponent @click="onLogin" :loading="isLoading" type="secondary">
                    <span class="align-middle">Log In</span>
                </BtnComponent>
                <p>
                    Don't have an account?
                    <RouterLink to="/signup">
                        <BtnComponent>Sign Up</BtnComponent>
                    </RouterLink>
                </p>
            </ContentCardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

import BtnComponent from '@/components/BtnComponent.vue';
import ContentCardComponent from '@/views/_shared/login/components/ContentCardComponent.vue';
import UserMessageComponent from '@/components/UserMessageComponent.vue';

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
</style>