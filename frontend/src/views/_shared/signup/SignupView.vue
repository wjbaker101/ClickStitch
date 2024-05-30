<template>
    <ViewComponent class="signup-view" hide-nav>
        <div class="mx-auto px-4 max-w-[720px]">
            <small>
                <RouterLink to="/login"><IconComponent icon="arrow-left" gap="right" />Return to login</RouterLink>
            </small>
            <ContentCardComponent>
                <h2><IconComponent icon="user" size="large" gap="right" />Sign up</h2>
                <label>
                    <strong>Email</strong>
                    <br>
                    <input ref="emailInput" type="text" v-model="email" placeholder="my@email.com" @keyup.enter="nextInput('passwordInput')" class="w-full">
                </label>
                <div class="my-4 grid gap-4 md:grid-cols-2">
                    <label>
                        <strong>Password</strong>
                        <br>
                        <input ref="passwordInput" type="password" v-model="password" placeholder="Password" @keyup.enter="nextInput('confirmPasswordInput')" class="w-full">
                    </label>
                    <label class="confirm-password">
                        <strong>Confirm Password</strong>
                        <br>
                        <input
                            ref="confirmPasswordInput"
                            type="password"
                            v-model="confirmPassword"
                            placeholder="Confirm Password"
                            @keyup.enter="nextInput('emailInput')"
                            class="w-full"
                        >
                    </label>
                </div>
                <UserMessageComponent ref="userMessageComponent" />
                <BtnComponent @click="onSignup" :loading="isLoading" type="secondary">
                    <span class="align-middle">Sign Up</span>
                </BtnComponent>
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

const router = useRouter();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const emailInput = ref<HTMLInputElement>({} as HTMLInputElement);
const passwordInput = ref<HTMLInputElement>({} as HTMLInputElement);
const confirmPasswordInput = ref<HTMLInputElement>({} as HTMLInputElement);

const inputs = {
    emailInput,
    passwordInput,
    confirmPasswordInput,
};

const email = ref<string>('');
const password = ref<string>('');
const confirmPassword = ref<string>('');

const isLoading = ref<boolean>(false);

const nextInput = async function (next: 'emailInput' | 'passwordInput' | 'confirmPasswordInput'): Promise<void> {
    const input = inputs[next].value;
    if (input.value.length === 0) {
        input.focus();
        return;
    }

    await onSignup();
};

const onSignup = async function () {
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
    if (confirmPassword.value.length === 0) {
        userMessageComponent.value.set('Please confirm your password.');
        return;
    }

    isLoading.value = true;

    const result = await api.users.createUser({
        email: email.value,
        password: password.value,
    });

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message);
        return;
    }

    await router.push({ path: '/login' });
};
</script>

<style lang="scss">
</style>