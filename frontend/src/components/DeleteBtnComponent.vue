<template>
    <BtnComponent type="custom" @click="onClick" @blur="onBlur" class="bg-danger hover:outline-danger focus:outline-danger">
        <template v-if="isConfirming">
            <CheckIcon class="mr-2" />
            <span class="align-middle">Confirm</span>
        </template>
        <template v-else>
            <DeleteIcon class="mr-2" />
            <span class="align-middle">Delete</span>
        </template>
    </BtnComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { Trash2 as DeleteIcon, CheckIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';

const emit = defineEmits(['delete']);

const isConfirming = ref<boolean>(false);

const onClick = function (): void {
    if (!isConfirming.value) {
        isConfirming.value = true;
        return;
    }

    emit('delete');
    isConfirming.value = false;
};

const onBlur = function (): void {
    isConfirming.value = false;
};
</script>