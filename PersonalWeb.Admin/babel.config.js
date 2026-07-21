export default {
    presets: [
        ['@babel/preset-env', {
            targets: {
                browsers: ['>0.2%', 'last 2 versions', 'not dead', 'not ie <= 11']
            }
        }]
    ]
}
