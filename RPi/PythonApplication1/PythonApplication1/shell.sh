#!/bin/bash

#echo "Recording (auto stop in 2 sec.)..."
arecord -D "plughw:1,0" --duration=5 -q -c 1 -f S16_LE --rate=16000 -t wav record_mono.wav
#arecord -D "plughw:1,0" --duration=5 -q -f cd -t wav -r 16000 record.wav
#sox record.wav -c 1 record_mono.wav
# timeout 2 parecord record.flac --verbose --volume=65535 --channels=2 --file-format=flac --rate=44100


#echo "Processing..."
# wget -q -U "Mozilla/5.0" --post-file record.flac --header "Content-Type: audio/x-flac; rate=44100" -O - "https://www.google.com/speech-api/v2/recognize?client=chromium&lang=en-us&key=AIzaSyDGdH6lhqaZ0ohKj95czvAZ5nK_4pJEqkw&output=json" | cut -d\" -f12  > speech.txt
# curl -X POST --data-binary @record.flac --header 'Content-Type: audio/x-flac; rate=44100;' 'https://www.google.com/speech-api/v2/recognize?output=json&lang=en-us&key=AIzaSyDGdH6lhqaZ0ohKj95czvAZ5nK_4pJEqkw&client=chromium&maxresults=6&pfilter=2'
curl -X POST --data-binary @'record_mono.wav' --header 'Content-Type: audio/l16; rate=16000;' 'https://www.google.com/speech-api/v2/recognize?output=json&lang=en-us&key=AIzaSyDGdH6lhqaZ0ohKj95czvAZ5nK_4pJEqkw&client=chromium&maxresults=6&pfilter=2'

#echo -n "You said: "
#cat speech.txt

# rm record.flac


# echo "Listening..."

# sudo arecord -D plughw:1,0 -f cd -t wav -d 3 -r 16000 | flac - -f --best --sample-rate 16000 -o record.flac

# timeout 3 parecord record.flac --verbose --volume=65535 --channels=2 --file-format=flac --rate=16000

# echo "Cotacting Server..."
# sudo wget -q --post-file record.flac --header="Content-Type:audio/x-flac; rate=16000" -O - "https://www.google.com/speech-api/v2/recognize?client=chromium&lang=en_US&key=AIzaSyB-oMuEp7pr6wShCnH6Rwn6VXK1LXRneEY"
