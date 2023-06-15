type Subdomain = 'creator';

export const routerHelper = {

    subdomain(): Subdomain | null {
        const split = window.location.host.split('.');
        if (split.length < 3)
            return null;

        split.pop();
        split.pop();

        return split.join('.') as Subdomain;
    },

};