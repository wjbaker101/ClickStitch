<template>
    <div class="login-view flex">
        <div class="left-side flex">
            <div class="centered flex-auto">
                <h1>ClickStitch</h1>
                <h2>Log In</h2>
                <p>
                    <label>
                        <strong>Username</strong>
                        <br>
                        <input type="text" v-model="username">
                    </label>
                </p>
                <p>
                    <label>
                        <strong>Password</strong>
                        <br>
                        <input type="password" v-model="password">
                    </label>
                </p>
                <ButtonComponent class="tertiary" @click="onLogin">Log In</ButtonComponent>
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
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

import { api } from '@/api/api';

import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();
const router = useRouter();

const username = ref<string>('');
const password = ref<string>('');

const onLogin = async function () {
    const result = await api.auth.logIn({
        username: username.value,
        password: password.value,
    });

    auth.set({
        username: '',
        loginToken: result.loginToken,
    });

    await router.push({ path: '/dashboard' });
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.login-view {
    height: 100%;

    $angle: 2rem;

    h1 {
        margin: 0 0 3rem 0;
    }

    h2 {
        margin: 0;
    }

    .left-side {
        position: relative;
        background-color: var(--wjb-primary);
        background: linear-gradient(
            -45deg,
            var(--wjb-primary-dark),
            var(--wjb-primary),
        );
        margin-right: -$angle;
        clip-path: polygon(0 0, 100% 0, calc(100% - ($angle * 2)) 100%, 0% 100%);
        filter: drop-shadow(1px 2px 3px #fff);
    }

    .right-side {
        overflow-y: auto;
        background-color: var(--wjb-background-colour);

        .description {
            margin: 0.5rem 0 2rem 0;
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
        background-color: #fff;
        border-radius: var(--wjb-border-radius);

        @include shadow-small();
    }
}
</style>