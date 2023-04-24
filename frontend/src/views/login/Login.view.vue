<template>
    <ViewComponent class="login-view" hide-nav>
        <div class="left-side flex">
            <div class="centered flex-auto">
                <h1>ClickStitch</h1>
                <h2>Log In</h2>
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
            </div>
        </div>
        <div class="right-side flex">
            <div class="right-side-content centered flex-auto">
                <h2>Check out these Patterns!</h2>
                <p class="description">
                    <IconComponent icon="arrow-left" gap="right" />
                    <span>Log in to be able to add them to your account</span>
                </p>
                <div class="example-patterns">
                    <img class="example-pattern" src="@/assets/templar-knight.png">
                    <img class="example-pattern" src="@/assets/howling-wolf.jpg">
                    <img class="example-pattern" src="@/assets/howling-wolf.jpg">
                    <img class="example-pattern" src="@/assets/templar-knight.png">
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
    height: 100%;

    $angle: 4rem;

    h1 {
        margin: 0 0 3rem 0;
    }

    h2 {
        margin: 0;
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
        clip-path: polygon(0 0, 100% 0, calc(100% - ($angle * 2)) 100%, 0% 100%);
        z-index: 1;

        @media screen and (max-width: 720px) {
            margin-right: 0;
            position: static;
            padding: 2rem 0;
            clip-path: none;
        }
    }

    .right-side {
        inset: 0 0 0 50%;
        position: absolute;
        background-color: var(--wjb-background-colour);

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
    }
}
</style>