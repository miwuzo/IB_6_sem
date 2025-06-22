const express = require('express');
const path = require('path');
const { encrypt, decrypt, avalancheEffect, stepByStepAvalancheEffect } = require('./des-ede2');

const app = express();
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'views'));
app.use(express.static('views'));

function normalizeKey(input) {
    let keyBuffer = Buffer.from(input, 'utf8');
    if (keyBuffer.length < 8) {
        const padding = Buffer.alloc(8 - keyBuffer.length);
        keyBuffer = Buffer.concat([keyBuffer, padding]);
    } else if (keyBuffer.length > 8) {
        keyBuffer = keyBuffer.subarray(0, 8);
    }
    return keyBuffer;
}

function createTripleDESKey(key1, key2, key3) {
    return Buffer.concat([normalizeKey(key1), normalizeKey(key2), normalizeKey(key3)]);
}

app.post('/encrypt', (req, res) => {
    const { enc_text, key1, key2, key3 } = req.body;
    const tripleDESKey = createTripleDESKey(key1, key2, key3);

    try {
        const startEnc = performance.now();
        const encrypted = encrypt(enc_text, tripleDESKey);
        const encTime = (performance.now() - startEnc).toFixed(2);

        const startDec = performance.now();
        const decrypted = decrypt(encrypted, tripleDESKey);
        const decTime = (performance.now() - startDec).toFixed(2);

        const avalanche = avalancheEffect(enc_text, tripleDESKey);
        const avalancheSteps = stepByStepAvalancheEffect(enc_text, tripleDESKey);

        res.json({
            encrypted,
            decrypted,
            encodingTime: encTime,
            decodingTime: decTime,
            avalanche,
            steps: avalancheSteps
        });
    }
    catch (e) {
        console.error(e);
        res.status(500).json({ error: 'Ошибка шифрования/дешифрования' });
    }
});

app.get('/', (req, res) => {
    res.render('des-ede2');
});

app.post('/test-s-weak-keys', (req, res) => {
    const { key1, key2, key3 } = req.body;
    const testInput1 = "Test input 1";
    const testInput2 = "Test input 2";

    const tripleDESKey = createTripleDESKey(key1, key2, key1);
    
    const encrypted1 = encrypt(testInput1, tripleDESKey);
    const encrypted2 = encrypt(testInput2, tripleDESKey);

    const isWeak = encrypted1 === encrypted2;

    res.json({
        isWeak,
        encrypted1,
        encrypted2
    });
});

app.post('/test-weak-keys', (req, res) => {
    const { key1, key2 } = req.body;
    const testInput = "Test input";

    const tripleDESKey1 = createTripleDESKey(key1, key1, key1);
    const tripleDESKey2 = createTripleDESKey(key2, key2, key2);

    const encrypted1 = encrypt(testInput, tripleDESKey1);
    const encrypted2 = encrypt(testInput, tripleDESKey2);

    const isSemiWeak = encrypted1 === encrypted2;

    res.json({
        isSemiWeak,
        encrypted1,
        encrypted2
    });
});

app.listen(3000, () => console.log('Сервер запущен: http://localhost:3000'));