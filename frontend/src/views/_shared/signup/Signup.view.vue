<template>
    <ViewComponent class="signup-view" hide-nav>
        <div class="content-width">
            <h1 class="flex gap-small align-items-center">
                <div class="logo-container flex-auto">
                    <img width="" src="@/assets/logo.png">
                </div>
                <span>ClickStitch</span>
            </h1>
            <small>
                <RouterLink to="/login"><IconComponent icon="arrow-left" gap="right" />Return to login</RouterLink>
            </small>
            <ContentCardComponent>
                <h2><IconComponent icon="user" size="large" gap="right" />Sign up</h2>
                <p>
                    <label>
                        <strong>Email</strong>
                        <br>
                        <input ref="emailInput" type="text" v-model="email" placeholder="my@email.com" @keyup.enter="nextInput('passwordInput')">
                    </label>
                </p>
                <p class="passwords-container flex gap">
                    <label>
                        <strong>Password</strong>
                        <br>
                        <input ref="passwordInput" type="password" v-model="password" placeholder="Password" @keyup.enter="nextInput('confirmPasswordInput')">
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
                        >
                    </label>
                </p>
                <UserMessageComponent ref="userMessageComponent" />
                <ButtonComponent class="tertiary" @click="onSignup" :loading="isLoading">Sign Up</ButtonComponent>
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
@use '@/style/variables' as *;

.signup-view {

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

    .passwords-container {
        @media screen and (max-width: 720px) {
            display: block;

            .confirm-password {
                display: block;
                margin-top: 1rem;
            }
        }
    }

    input {
        width: 100%;
    }
}
</style>