<template>
    <ViewComponent class="signup-view" hide-nav>
        <div class="left-side flex flex-vertical">
            <header class="flex flex-auto gap">
                <div></div>
                <div class="flex-auto">
                    <RouterLink to="/about">
                        <IconComponent icon="info" gap="right" />
                        <span>About ClickStitch</span>
                    </RouterLink>
                </div>
                <div class="flex-auto">
                    <RouterLink to="/patterns">
                        <IconComponent icon="download" gap="right" />
                        <span>View Patterns</span>
                    </RouterLink>
                </div>
            </header>
            <div class="centered flex-auto">
                <h1 class="flex gap-small align-items-center">
                    <div class="logo-container flex-auto">
                        <img width="" src="@/assets/logo.png">
                    </div>
                    <span>ClickStitch</span>
                </h1>
                <p class="back-to-login">
                    <RouterLink class="back-to-login" to="/login">
                        <IconComponent icon="arrow-left" /> Back to login
                    </RouterLink>
                </p>
                <h2>Sign up</h2>
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
            </div>
        </div>
        <div class="right-side flex textured-background">
            <div class="right-side-content centered flex-auto">
                <h2>So close to getting these! <IconComponent icon="arrow-down" /></h2>
                <p class="description">
                    <span>Complete signup and add them to your account</span>
                </p>
                <div class="example-patterns">
                    <img class="example-pattern" src="@/assets/examples/templar-knight.png">
                    <img class="example-pattern" src="@/assets/examples/howling-wolf.jpg">
                    <img class="example-pattern" src="@/assets/examples/bat-silhouette.png">
                    <img class="example-pattern" src="@/assets/examples/dragon-silhouette.png">
                </div>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

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
    height: 100%;

    $angle: 4rem;

    h1 {
        margin: 0 0 3rem 0;
    }

    h2 {
        margin: 0;
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

    .left-side {
        inset: 0 50% 0 0;
        position: absolute;
        background-color: var(--wjb-primary);
        background: linear-gradient(
            -45deg,
            var(--wjb-primary-dark),
            var(--wjb-primary),
        );
        margin-right: -$angle;
        color: var(--wjb-light);
        clip-path: polygon(0 0, 100% 0, calc(100% - ($angle * 2)) 100%, 0% 100%);
        z-index: 1;

        @media screen and (max-width: 720px) {
            margin-right: 0;
            position: static;
            padding: 2rem 0;
            clip-path: none;
        }

        header {
            padding: 1rem;
            margin-right: 1rem;

            a {
                color: inherit;
                text-decoration: none;

                &:hover {
                    text-decoration: underline;
                }
            }
        }
    }

    .back-to-login {
        margin-top: -2rem;
        color: inherit;
        text-decoration: none;

        &:hover {
            text-decoration: underline;
        }
    }

    .right-side {
        inset: 0 0 0 50%;
        position: absolute;
        overflow-y: auto;

        .description {
            margin: 0.5rem 0 2rem 0;
        }

        @media screen and (max-width: 720px) {
            position: static;
            padding: 2rem 0;
            background-color: transparent;
        }
    }

    .right-side-content {
        padding-left: $angle;
    }

    .centered {
        margin: auto;
    }

    .passwords-container {
        @media screen and (max-width: 1280px) {
            flex-direction: column;
        }
    }

    .example-patterns {
        width: calc(300px + 1rem);
        display: grid;
        gap: 1rem;
        grid-template: 1fr 1fr / 1fr 1fr;
    }

    .example-pattern {
        border-radius: var(--wjb-border-radius);

        @include shadow-small();
    }

    .footer-component {
        position: relative;
        color: #ddd;
        z-index: 1;

        a {
            color: var(--wjb-light);
            box-shadow: 0 1px 0px var(--wjb-light);

            &:hover {
                box-shadow: none;
            }
        }
    }
}
</style>