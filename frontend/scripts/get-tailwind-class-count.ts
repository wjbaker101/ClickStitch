import { readdir } from 'node:fs/promises';

const path = __dirname + '/../src/';
const files = await readdir(path, { recursive: true });

const regex = /[^:]class="([^"]+)"/gi;
const counts = new Map<string, number>();

for (const filePath of files) {
    const file = Bun.file(path + filePath);

    if (!await file.exists())
        continue;

    const contents = await file.text();
    const matches = [...contents.matchAll(regex)].map(x => x[1]);

    if (!matches)
        continue;

    for (const match of matches) {
        const classList = match.split(' ');

        for (const _class of classList) {
            const cleanedClass = _class.trim();

            if (cleanedClass.length === 0)
                continue;

            if (!counts.has(cleanedClass))
                counts.set(cleanedClass, 0)

            counts.set(cleanedClass, counts.get(cleanedClass) as number + 1);
        }
    }
}

const countsSorted = [...counts.entries()].sort((a, b) => b[1] - a[1]);

console.log(countsSorted.map(x => x[0] + ' -> ' + x[1]).join('\n'));