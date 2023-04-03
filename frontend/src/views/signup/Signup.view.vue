<template>
    <div class="login-view flex">
        <div class="left-side flex">
            <div class="centered flex-auto">
                <h1>ClickStitch</h1>
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
                        <input type="text" v-model="email" placeholder="my@email.com">
                    </label>
                </p>
                <p class="flex gap">
                    <label>
                        <strong>Password</strong>
                        <br>
                        <input type="password" v-model="password" placeholder="Password">
                    </label>
                    <label>
                        <strong>Confirm Password</strong>
                        <br>
                        <input type="password" v-model="confirmPassword" placeholder="Confirm Password">
                    </label>
                </p>
                <UserMessageComponent ref="userMessageComponent" />
                <ButtonComponent class="tertiary" @click="onLogin">Sign Up</ButtonComponent>
            </div>
        </div>
        <div class="right-side flex">
            <div class="right-side-content centered flex-auto">
                <h2>So close to getting these! <IconComponent icon="arrow-down" /></h2>
                <p class="description">
                    <span>Complete signup and add them to your account</span>
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

import UserMessageComponent from '@/components/UserMessage.component.vue';

import { api } from '@/api/api';

const router = useRouter();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const email = ref<string>('');
const password = ref<string>('');
const confirmPassword = ref<string>('');

const onLogin = async function () {
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

    await api.users.createUser({
        email: email.value,
        password: password.value,
    });

    await router.push({ path: '/login' });
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

    .back-to-login {
        margin-top: -2rem;
        color: inherit;
        text-decoration: none;

        &:hover {
            text-decoration: underline;
        }
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
        border-radius: var(--wjb-border-radius);

        @include shadow-small();
    }
}
</style>